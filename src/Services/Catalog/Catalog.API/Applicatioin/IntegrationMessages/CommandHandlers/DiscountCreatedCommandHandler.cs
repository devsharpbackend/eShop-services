using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.CatalogAPI.Application.IntegrationMessages.CommandHandlers;
public class DiscountCreatedCommandHandler : IConsumer<DiscountCreatedIntegrationCommand>
{

    private readonly ILogger<DiscountCreatedCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CatalogErrorHandler _integrationEventErrorHandler;
    public DiscountCreatedCommandHandler(ILogger<DiscountCreatedCommandHandler> logger, IMediator mediator, CatalogErrorHandler integrationEventErrorHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _integrationEventErrorHandler = integrationEventErrorHandler ?? throw new ArgumentNullException(nameof(integrationEventErrorHandler));
    }

    public async Task Consume(ConsumeContext<DiscountCreatedIntegrationCommand> context)
    {

        try
        {
            var @event = context.Message;
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {

                var command = new UpdateDiscountCommand
                {
                    Amount = @event.Amount,
                    Id = @event.CatalogID
                };
                _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}:  ({@Command})",
                command.GetGenericTypeName(),
                nameof(command.Id),

                command);
                await _mediator.Send(command);

                await context.RespondAsync(new CreateDiscountValidResponse { });
            }
        }
        catch (CatalogApplicationException ex)
        {
            _logger.LogError("Error in DiscountCreatedEventHandler ");

            await context.RespondAsync(ex.JsonErrorResponse);
        }
    }
}
