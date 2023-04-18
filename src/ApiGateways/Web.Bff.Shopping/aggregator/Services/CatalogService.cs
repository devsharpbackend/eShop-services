using CatalogGrpc;

namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Services;

public class CatalogService : ICatalogService
{
    private readonly Catalog.CatalogClient _client;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(Catalog.CatalogClient client, ILogger<CatalogService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<CatalogItem> GetCatalogItemAsync(int id)
    {
        var request = new CatalogItemRequest { Id = id };
        _logger.LogInformation("grpc request {@request}", request);
        var response = await _client.GetItemByIdAsync(request);
        _logger.LogInformation("grpc response {@response}", response);
        return MapToCatalogItemResponse(response);

    }

    public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(IEnumerable<int> ids)
    {
        string strIds = "";
        foreach (var id in ids)
        {
            strIds += id + ",";
        }
        var request = new CatalogItemsIdsRequest { Ids = strIds };
        _logger.LogInformation("grpc request {@request}", request);
        var response = await _client.GetItemsByIdsAsync(request);

        _logger.LogInformation("grpc response {@response}", response);
        return response.Data.Select(this.MapToCatalogItemResponse);
    }

    private CatalogItem MapToCatalogItemResponse(CatalogItemResponse catalogItemResponse)
    {

        return new CatalogItem
        {
            Id = catalogItemResponse.Id,
            Name = catalogItemResponse.Name,
            Price = (decimal)catalogItemResponse.Price,
            PictureUri = catalogItemResponse.PictureUrl
        };
    }
}
