namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.UpdateBasket;

public class UpdateBasketCommand : IRequest
{
    public string BuyerId { get; set; }
    public List<BasketItemVM> BasketItems { get; set; }
}


