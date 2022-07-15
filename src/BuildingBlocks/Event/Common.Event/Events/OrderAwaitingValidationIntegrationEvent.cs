namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record OrderAwaitingValidationIntegrationEvent: IntegrationEvent
{
    public int OrderId { get; }
    public Guid OrderNumber { get; }
    public int OrderStatus { get; }
    public string BuyerName { get; }
   

    public OrderAwaitingValidationIntegrationEvent(int orderId, Guid orderNumber, int orderStatus, string buyerName)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}
