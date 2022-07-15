namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public class GetUserOrderListQuery: IRequest<IEnumerable<OrderSummaryVM>>
{
    public string UserId { get; set; }
}
