namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure.Models;

public class BasketDataItem
{
    public string Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal OldUnitPrice { get; set; }

    public int Quantity { get; set; }

    public string PictureUrl { get; set; }
}
