namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Services;

public interface IBasketService
{
    Task<BasketData> GetByIdAsync(string id);

    Task UpdateAsync(BasketData currentBasket);
}
