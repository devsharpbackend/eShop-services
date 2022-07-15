namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.UpdateBasketCatalogPrice;

public class UpdateBasketCatalogPriceCommandHandler : IRequestHandler<UpdateBasketCatalogPriceCommand>
{
    private readonly IBasketRepository _repository;

    private readonly ILogger<UpdateBasketCatalogPriceCommandHandler> _logger;

    public UpdateBasketCatalogPriceCommandHandler(
        ILogger<UpdateBasketCatalogPriceCommandHandler> logger,
        IBasketRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateBasketCatalogPriceCommand request, CancellationToken cancellationToken)
    {

        var userIds = _repository.GetUsers();

        foreach (var id in userIds)
        {
            var basket = await _repository.GetBasketAsync(id);

            await UpdatePriceInBasketItems(request.CatalogId, request.NewPrice, request.OldPrice, basket);
        }
        return Unit.Value;
    }

    private async Task UpdatePriceInBasketItems(int productId, decimal newPrice, decimal oldPrice, CustomerBasket basket)
    {
        var itemsToUpdate = basket?.Items?.Where(x => x.ProductId == productId).ToList();

        if (itemsToUpdate != null)
        {
            _logger.LogInformation("----- ProductPriceChangedIntegrationEventHandler - Updating items in basket for user: {BuyerId} ({@Items})", basket.BuyerId, itemsToUpdate);

            foreach (var item in itemsToUpdate)
            {
                if (item.UnitPrice == oldPrice)
                {
                    var originalPrice = item.UnitPrice;
                    item.SetNewPrice(newPrice);
                }
            }
            await _repository.UpdateBasketAsync(basket);
        }
    }

}