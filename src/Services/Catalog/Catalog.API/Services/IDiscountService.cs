namespace eShop.Services.Catalog.CatalogAPI.Services;

public interface IDiscountService
{
    Task<DiscountItemDto?> GetDiscounts(int catalogId);
}
