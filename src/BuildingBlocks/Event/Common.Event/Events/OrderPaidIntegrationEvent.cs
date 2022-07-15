namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record OrderPaidIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }
   

    public OrderPaidIntegrationEvent(int orderId,
        string orderStatus,
        string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}

