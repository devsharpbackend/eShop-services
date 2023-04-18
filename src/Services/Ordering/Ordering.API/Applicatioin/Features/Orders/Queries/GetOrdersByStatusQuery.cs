namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public class GetOrdersByStatusQuery : IRequest<IEnumerable<OrderVM>>
{
  public  OrderStatus orderStatus { get; set; }
}