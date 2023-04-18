namespace eShop.Services.Ordering.OrderingAPI.Application.DomainEventHandlers.OrderStartedEvent;

public class OrderIntegrationEventWhenOrderStartedDomainEventHandler
                    : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly ILoggerFactory _logger;

    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public OrderIntegrationEventWhenOrderStartedDomainEventHandler(ILoggerFactory logger,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
    {
        var orderStatusChangedTosubmittedIntegrationEvent = new OrderStartedIntegrationEvent(orderStartedEvent.UserId,orderStartedEvent.Order.Id,(int) orderStartedEvent.Order.OrderStatus);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedTosubmittedIntegrationEvent);

    }
}
