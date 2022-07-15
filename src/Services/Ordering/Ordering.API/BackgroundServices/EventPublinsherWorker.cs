namespace eShop.Services.Ordering.OrderingAPI.Application.BackgroundServices;

public class EventPublinsherWorker : BackgroundService
{

    private readonly IServiceScopeFactory _serviceScopeFactory;
    public EventPublinsherWorker(IServiceScopeFactory serviceScopeFactory)
    {
       _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var _logger =scope.ServiceProvider.GetRequiredService<ILogger<EventPublinsherWorker>>();
        var _catalogIntegrationEventService=scope.ServiceProvider.GetRequiredService<IOrderingIntegrationEventService>();
        var _options=scope.ServiceProvider.GetRequiredService<IOptions<OrderSetting>>();

        _logger.LogDebug("GracePeriodManagerService is starting.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("GracePeriodManagerService background task is doing background work.");

            await _catalogIntegrationEventService.PublishUnSendedEventsThroughEventBusAsync();

            await Task.Delay(_options.Value.CheckUpdateTimeForEventPublinsherWorker, stoppingToken);

        }

        _logger.LogDebug("GracePeriodManagerService background task is stopping.");
        stoppingToken.Register(() => _logger.LogDebug("#1 GracePeriodManagerService background task is stopping."));
    }

}