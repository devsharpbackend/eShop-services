namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;

public class SetCompletedOrderStatusCommand : IRequest
{

    [DataMember]
    public int OrderId { get; private set; }

    public SetCompletedOrderStatusCommand(int OrderId)
    {
        this.OrderId = OrderId;
    }
}
