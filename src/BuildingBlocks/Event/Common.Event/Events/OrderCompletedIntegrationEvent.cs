namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record OrderCompletedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public string OrderStatus { get; }
    public string BuyerName { get; }
   

    public OrderCompletedIntegrationEvent(int orderId,
        string orderStatus,
        string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}

