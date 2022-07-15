namespace eShop.Services.CatalogAPI.Services;
using DiscountGrpc;
public class DiscountGrpcService : IDiscountService
{
    
    private readonly ILogger<DiscountGrpcService> _logger;
    private readonly CatalogDiscountGrpc.CatalogDiscountGrpcClient _catalogDiscountGrpcClient;

    public DiscountGrpcService(ILogger<DiscountGrpcService> logger, CatalogDiscountGrpc.CatalogDiscountGrpcClient catalogDiscountGrpcClient)
    {
        _logger = logger;
        _catalogDiscountGrpcClient = catalogDiscountGrpcClient;
    }

    public async Task<DiscountItemDto?> GetDiscounts(int catalogId)
    {

       var response=await _catalogDiscountGrpcClient.GetDiscountByCatalogIdAsync(new CatalogDiscountRequest { 
       CatalogId=catalogId
       });

        if(response==null)
        {
            return new DiscountItemDto
            {
                Amount = -20
            };
        }
        else
        {
            return new DiscountItemDto
            {
                Amount = (decimal)response.Amount
            };
        }
       
    }
}
