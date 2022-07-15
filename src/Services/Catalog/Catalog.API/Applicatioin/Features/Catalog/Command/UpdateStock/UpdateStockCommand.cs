
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Commands.UpdateStock;

public class UpdateStockCommand:IRequest<int>
{
    public int Id { get; set; }
   
    public int Quantity { get; set; }
}
