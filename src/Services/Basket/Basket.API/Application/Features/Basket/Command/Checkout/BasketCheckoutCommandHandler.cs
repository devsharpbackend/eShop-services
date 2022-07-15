
namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.Checkout;

public class BasketCheckoutCommandHandler : IRequestHandler<BasketCheckoutCommand>
{
    private readonly IBasketRepository _repository;
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutCommandHandler> _logger;
    private readonly IRequestClient<UserCheckoutAcceptedIntegrationCommand> _userCheckoutClient;
    private readonly IBasketIdentityService _identityService;

    public BasketCheckoutCommandHandler(
        ILogger<BasketCheckoutCommandHandler> logger, IMediator mediator, IRequestClient<UserCheckoutAcceptedIntegrationCommand> userCheckoutClient,
        IBasketRepository repository, IBasketIdentityService identityService
        )
    {
        _logger = logger;
        _repository = repository;
        _mediator = mediator;
        _userCheckoutClient = userCheckoutClient;
        _identityService = identityService;
    }

    public async Task<Unit> Handle(BasketCheckoutCommand basketCheckout, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserIdentity();

       

        var basket = await _mediator.Send(new GetCustomerBasketQuery { CustomerId = userId });

        if (basket == null)
        {
            throw new NotFoundException(nameof(basket), basket.BuyerId);
        }

        var userName = "morteza";//this.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;


        // Conveert Basket Info to CustomerBasketIntegrationCommand
        CustomerBasketIntegrationCommand customerBasketIntegration = new CustomerBasketIntegrationCommand
        {
            BuyerId = basket.BuyerId,
            BasketItems = new List<BasketItemIntegrationCommand>()
        };


        foreach (var item in basket.BasketItems)
        {
            customerBasketIntegration.BasketItems.Add(new BasketItemIntegrationCommand
            {
                Id = item.Id,
                OldUnitPrice = item.OldUnitPrice,
                PictureUrl = item.PictureUrl,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
            });
        }


        var command = new UserCheckoutAcceptedIntegrationCommand(userId, userName, basketCheckout.City, basketCheckout.Street,
            basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName,
            basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.Buyer, basketCheckout.RequestId, customerBasketIntegration);


        // Send UserCheckoutAcceptedIntegrationCommand To Ordering MicroService
        var response = await _userCheckoutClient.GetResponse<CreateOrderResponse, JsonErrorResponse>(command);



        // If Ordering Microservice was Rejectd Current Request
        if (response.Is(out Response<JsonErrorResponse> responseA))
        {
            throw new BasketApplicationException(responseA.Message);
        };
        return Unit.Value;
    }

  
}