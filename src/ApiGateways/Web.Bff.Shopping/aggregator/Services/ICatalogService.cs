namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Services;

public interface ICatalogService
{
    Task<CatalogItem> GetCatalogItemAsync(int id);

    Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(IEnumerable<int> ids);
}