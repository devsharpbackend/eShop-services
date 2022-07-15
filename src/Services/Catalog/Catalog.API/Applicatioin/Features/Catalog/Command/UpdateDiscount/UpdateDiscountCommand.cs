namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Command.UpdateDiscount;

public class UpdateDiscountCommand:IRequest
{
    public int Id { get; set; }
    public decimal Amount { get; set; }

}
