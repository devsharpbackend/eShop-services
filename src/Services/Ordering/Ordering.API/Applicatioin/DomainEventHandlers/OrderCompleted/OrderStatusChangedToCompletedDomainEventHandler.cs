namespace eShop.Services.Ordering.OrderingAPI.Application.DomainEventHandlers.OrderPaid;
    
public class OrderStatusChangedToCompletedDomainEventHandler
                : INotificationHandler<OrderStatusChangedToCompletedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;


    public OrderStatusChangedToCompletedDomainEventHandler(
        IOrderRepository orderRepository, ILoggerFactory logger,
        IBuyerRepository buyerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService  )
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
    }

    public async Task Handle(OrderStatusChangedToCompletedDomainEvent changedToCompletedDomainEvent, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderStatusChangedToPaidDomainEventHandler>()
            .LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",
                changedToCompletedDomainEvent.Order.Id, nameof(OrderStatus.Completed), OrderStatus.Completed);

        var order = await _orderRepository.GetAsync(changedToCompletedDomainEvent.Order.Id);
        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value.ToString());

        await _orderingIntegrationEventService.AddAndSaveEventAsync(new
          OrderCompletedIntegrationEvent(order.Id, order.OrderStatus.ToString(), buyer.Name));
    }
}
