namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.UpdateBasket;

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand>
{
    private readonly IBasketRepository _repository;

    private readonly ILogger<BasketCheckoutCommandHandler> _logger;

    public UpdateBasketCommandHandler(
        ILogger<BasketCheckoutCommandHandler> logger,
        IBasketRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
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