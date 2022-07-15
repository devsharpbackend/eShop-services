namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Models;

public class UpdateBasketItemsRequest
{
    public string BasketId { get; set; }

    public ICollection<UpdateBasketItemData> Updates { get; set; }

    public UpdateBasketItemsRequest()
    {
        Updates = new List<UpdateBasketItemData>();
    }
}
