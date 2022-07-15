namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Models;

public class UpdateBasketRequest
{
    public string BuyerId { get; set; }

    public IEnumerable<UpdateBasketRequestItemData> Items { get; set; }
}
