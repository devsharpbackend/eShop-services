namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;


public class SetStockConfirmedOrderStatusCommand : IRequest<bool>
{

    [DataMember]
    public int OrderId { get; private set; }

    public SetStockConfirmedOrderStatusCommand(int orderId)
    {
        OrderId = orderId;
    }
}
