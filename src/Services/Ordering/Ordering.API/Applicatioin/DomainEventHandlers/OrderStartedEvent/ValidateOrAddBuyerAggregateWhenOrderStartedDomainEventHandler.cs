namespace eShop.Services.Ordering.OrderingAPI.Application.DomainEventHandlers.OrderStartedEvent;

public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler
                    : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly ILoggerFactory _logger;
    private readonly IBuyerRepository _buyerRepository;

    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
        ILoggerFactory logger,
        IBuyerRepository buyerRepository )
    {
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;

        var buyer = await _buyerRepository.FindAsync(orderStartedEvent.UserId);
        bool buyerOriginallyExisted = (buyer == null) ? false : true;

        if (!buyerOriginallyExisted)
        {
            buyer = new Buyer(orderStartedEvent.UserId, orderStartedEvent.UserName);
        }

        // Assign Payment Methods to Buyers
        buyer.VerifyOrAddPaymentMethod(cardTypeId,
                                        $"Payment Method on {DateTime.UtcNow}",
                                        orderStartedEvent.CardNumber,
                                        orderStartedEvent.CardSecurityNumber,
                                        orderStartedEvent.CardHolderName,
                                        orderStartedEvent.CardExpiration,
                                        orderStartedEvent.Order.Id);



        var buyerUpdated = buyerOriginallyExisted ?
            _buyerRepository.Update(buyer) :
            _buyerRepository.Add(buyer);

        await _buyerRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

      
        _logger.CreateLogger<OrderIntegrationEventWhenOrderStartedDomainEventHandler>()
            .LogTrace("Buyer {BuyerId} and related payment method were validated or updated for orderId: {OrderId}.",
                buyerUpdated.Id, orderStartedEvent.Order.Id);
    }
}
