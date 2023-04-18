namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Queries.GetBasket;


public class CustomerBasketVM
{
    public string BuyerId { get; set; }
    public List<BasketItemVM> BasketItems { get; set; }
}

public class BasketItemVM
{
    public string Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal OldUnitPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
}
