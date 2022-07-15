namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;


public class CancelOrderCommand : IRequest<bool>
{

    [DataMember]
    public int OrderId { get; set; }
    public CancelOrderCommand()
    {

    }
    public CancelOrderCommand(int OrderId)
    {
        OrderId = OrderId;
    }
}
