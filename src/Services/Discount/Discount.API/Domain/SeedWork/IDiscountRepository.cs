namespace eShop.Services.Discount.DiscountAPI.Domain.SeedWork;


public interface IDiscountRepository
    {
        Task<CatalogDiscount> GetDiscount(int catalogId);
        Task<bool> CreateDiscount(CatalogDiscount discount);
        Task<bool> UpdateDiscount(CatalogDiscount discount);
        Task<bool> DeleteDiscount(int Id);
    }

