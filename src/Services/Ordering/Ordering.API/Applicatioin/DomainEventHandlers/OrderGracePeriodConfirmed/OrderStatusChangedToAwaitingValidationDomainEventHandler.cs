using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Ordering.OrderingAPI.Application.DomainEventHandlers.OrderGracePeriodConfirmed;
    
public class OrderStatusChangedToAwaitingValidationDomainEventHandler
                : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    public OrderStatusChangedToAwaitingValidationDomainEventHandler(
        IOrderRepository orderRepository, ILoggerFactory logger,
        IBuyerRepository buyerRepository
      ,  IOrderingIntegrationEventService orderingIntegrationEventService
        )
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent orderStatusChangedToAwaitingValidationDomainEvent, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<OrderStatusChangedToAwaitingValidationDomainEvent>()
            .LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})",
                orderStatusChangedToAwaitingValidationDomainEvent.OrderId, nameof(OrderStatus.AwaitingValidation), OrderStatus.AwaitingValidation);

       

        var order = await _orderRepository.GetAsync(orderStatusChangedToAwaitingValidationDomainEvent.OrderId);

        var buyer = await _buyerRepository.FindByIdAsync(order.GetBuyerId.Value.ToString());

        // Send Integration Command for Checkeing Stock
        

        //var orderStockList = orderStatusChangedToAwaitingValidationDomainEvent.OrderItems
        //    .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.GetUnits()));
        //var orderStatusChangedToAwaitingValidationIntegrationCommnad = new OrderAwaitingValidationIntegrationCommand(
        //  order.Id, order.OrderNumber, (int)order.OrderStatus, buyer.Name, orderStockList);
        //var response = await _requestOrderAwaitingValidationIntegrationCommand.GetResponse<JsonErrorResponse, ValidResponse>(orderStatusChangedToAwaitingValidationIntegrationCommnad);

        //if (response.Is(out Response<JsonErrorResponse> responseA))
        //{
        //    throw new OrderingApplicationException(responseA.Message);
        //};


        // Raise Integration Event for Notification

        var orderStatusChangedToAwaitingValidationIntegrationEvent = new OrderAwaitingValidationIntegrationEvent(
          order.Id, order.OrderNumber, (int)order.OrderStatus, buyer.Name);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedToAwaitingValidationIntegrationEvent);

        
        
    }
}
