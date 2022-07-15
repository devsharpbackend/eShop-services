namespace eShop.Services.CatalogAPI.Application.IntegrationMessages;

public class CatalogIntegrationEventService : ICatalogIntegrationEventService
{
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly CatalogContext _catalogContext;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<CatalogIntegrationEventService> _logger;

    public CatalogIntegrationEventService(IPublishEndpoint publishEndpoint,
        CatalogContext catalogContext,
        IntegrationEventLogContext eventLogContext,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
        ILogger<CatalogIntegrationEventService> logger)
    {
        _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _eventLogService = _integrationEventLogServiceFactory(_catalogContext.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvt in pendingLogEvents)
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                await _publishEndpoint.Publish(logEvt.IntegrationEvent,logEvt.IntegrationEvent.GetType());
                await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Program.AppName);

                await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
            }
        }
    }


    public async Task PublishUnSendedEventsThroughEventBusAsync()
    {
        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPublishedFailedToPublishAsync(5);

        foreach (var logEvt in pendingLogEvents)
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                await _publishEndpoint.Publish(logEvt.IntegrationEvent, logEvt.IntegrationEvent.GetType());
                await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Program.AppName);

                await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IIntegrationMessage evt)
    {
        _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

        await _eventLogService.SaveEventAsync(evt, _catalogContext.GetCurrentTransaction());
    }
}
