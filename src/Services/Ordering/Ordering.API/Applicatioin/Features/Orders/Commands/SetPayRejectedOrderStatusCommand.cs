namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;

public class SetPaidOrderStatusCommand : IRequest<bool>
{

    [DataMember]
    public int OrderId { get; private set; }

    public SetPaidOrderStatusCommand(int OrderId)
    {
        this.OrderId = OrderId;
    }
}
