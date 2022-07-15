namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;


public class SetStockRejectedOrderStatusCommand : IRequest<bool>
{

    [DataMember]
    public int OrderId { get; private set; }

    [DataMember]
    public List<int> OrderStockItems { get; private set; }

    public SetStockRejectedOrderStatusCommand(int OrderId, List<int> orderStockItems)
    {
        this.OrderId = OrderId;
        OrderStockItems = orderStockItems;
    }
}
