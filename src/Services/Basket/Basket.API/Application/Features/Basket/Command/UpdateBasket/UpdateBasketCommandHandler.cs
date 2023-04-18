namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.UpdateBasket;

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand>
{
    private readonly IBasketRepository _repository;

    private readonly ILogger<UpdateBasketCommandHandler> _logger;
    private readonly IBasketIdentityService _identityService;
    public UpdateBasketCommandHandler(
        ILogger<UpdateBasketCommandHandler> logger,
        IBasketRepository repository,IBasketIdentityService identityService)
    {
        _logger = logger;
        _repository = repository;
        _identityService = identityService;
    }

    public async Task<Unit> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
       // var userId = _identityService.GetUserIdentity();
        // mapping 
        var customerBasket = new CustomerBasket(request.BuyerId);
        foreach (var item in request.BasketItems)
        {
            customerBasket.AddBasketItem(item.Id, item.Quantity, item.ProductId, item.ProductName,
                item.PictureUrl, item.UnitPrice, item.OldUnitPrice);
        }
        await _repository.UpdateBasketAsync(customerBasket);
        
        return Unit.Value;
    }

  
}