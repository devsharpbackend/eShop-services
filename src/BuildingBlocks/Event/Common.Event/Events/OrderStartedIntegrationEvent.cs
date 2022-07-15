namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record OrderStartedIntegrationEvent : IntegrationEvent
{
    public int OrderId { get; }
    public int OrderStatus { get; }
    public string BuyerName { get; }
    public string UserId { get; }

    public OrderStartedIntegrationEvent(string userId,int orderId, int orderStatus)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        UserId = userId;
    }
}
