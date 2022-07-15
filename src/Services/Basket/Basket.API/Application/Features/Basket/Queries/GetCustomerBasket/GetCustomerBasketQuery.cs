namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Queries.GetBasket;


public class GetCustomerBasketQuery:IRequest<CustomerBasketVM>
{
   public string CustomerId { get; set; }
}
