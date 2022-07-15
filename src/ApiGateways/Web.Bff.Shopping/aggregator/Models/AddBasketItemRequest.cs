namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Models;

public class AddBasketItemRequest
{
    public int CatalogItemId { get; set; }

    public string BasketId { get; set; }

    public int Quantity { get; set; }

    public AddBasketItemRequest()
    {
        Quantity = 1;
    }
}

