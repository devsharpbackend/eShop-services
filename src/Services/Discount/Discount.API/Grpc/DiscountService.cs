using DiscountGrpc;
using Grpc.Core;

namespace eShop.Services.Discount.DiscountAPI.Grpc;

public class DiscountService: DiscountGrpc.CatalogDiscountGrpc.CatalogDiscountGrpcBase
{
    private readonly IDiscountRepository _repository;
    public DiscountService(IDiscountRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async override Task<CatalogItemDiscountResponse> GetDiscountByCatalogId(CatalogDiscountRequest request, ServerCallContext context)
    {
        var discount = await _repository.GetDiscount(request.CatalogId);

        return new CatalogItemDiscountResponse
        {
            Amount =(double) discount.Amount,
            CatalogId = discount.CatalogId,
            Description = discount.Description,
            Id = discount.Id,
            Name = discount.Name,
        };
    }
}
