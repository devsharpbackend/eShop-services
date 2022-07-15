namespace eShop.Services.Discount.DiscountAPI.Domain.Model;

public class CatalogDiscount:Entity
{
    public string Name { get; set; } 
    public int CatalogId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
