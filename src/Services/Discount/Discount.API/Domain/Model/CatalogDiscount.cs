namespace eShop.Services.Discount.DiscountAPI.Domain.Model;

public class CatalogDiscount
{
    public int Id { get; set; } 
    public string Name { get; set; } 
    public int CatalogId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}