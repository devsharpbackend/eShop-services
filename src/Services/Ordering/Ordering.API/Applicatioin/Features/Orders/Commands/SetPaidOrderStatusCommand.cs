namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;

public class SetPayRejectedOrderStatusCommand : IRequest
{

    [DataMember]
    public int OrderId { get; private set; }

    public SetPayRejectedOrderStatusCommand(int OrderId)
    {
        this.OrderId = OrderId;
    }
}
