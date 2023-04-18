namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.DeleteBasket;

public class DeleteBasketCommand : IRequest
{
    public string BuyerId { get; set; }
}
