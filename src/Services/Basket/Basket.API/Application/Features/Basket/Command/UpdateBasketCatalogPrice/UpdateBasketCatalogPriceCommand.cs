namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.UpdateBasketCatalogPrice;

public class UpdateBasketCatalogPriceCommand : IRequest
{
    public int CatalogId { get; set; }
    public decimal NewPrice { get; set; }
    public decimal OldPrice { get; set; }

}


