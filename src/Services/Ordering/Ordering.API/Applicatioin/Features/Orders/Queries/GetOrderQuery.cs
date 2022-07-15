namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public class GetOrderQuery:IRequest<OrderVM>
{
    public int OrderId { get; set; }
}
