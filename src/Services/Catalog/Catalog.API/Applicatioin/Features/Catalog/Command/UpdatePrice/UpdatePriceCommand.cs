namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Command.UpdatePrice;

public class UpdatePriceCommand:IRequest
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public decimal Price { get; set; }
}
