
namespace Microsoft.eShopOnContainers.Services.Catalog.API.IntegrationEvents.EventHandling;

public class OrderAwaitingValidationIntegrationCommandHandler :
    IConsumer<OrderAwaitingValidationIntegrationCommand>
{
  
    private readonly ILogger<OrderAwaitingValidationIntegrationCommandHandler> _logger;
    private readonly IMediator _mediator;
    public OrderAwaitingValidationIntegrationCommandHandler(
        IMediator mediator,
        ILogger<OrderAwaitingValidationIntegrationCommandHandler> logger)
    {
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<OrderAwaitingValidationIntegrationCommand> context)
    {
        var @event = context.Message;
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            var confirmedOrderStockItems = new List<VeifyCatalogStockItem>();

            foreach (var orderStockItem in @event.OrderStockItems)
            {
                var confirmedOrderStockItem = new VeifyCatalogStockItem(orderStockItem.CatalogId, orderStockItem.Units);
                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }

            VeifyCatalogStockCommand command = new VeifyCatalogStockCommand(@event.OrderId,@event.OrderNumber, confirmedOrderStockItems);
            await _mediator.Send(command);
        }
    }
}
