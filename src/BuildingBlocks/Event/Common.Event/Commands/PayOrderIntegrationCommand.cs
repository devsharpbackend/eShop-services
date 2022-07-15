namespace eShop.BuildingBlocks.Event.CommonEvent.Commands;

public record PayOrderIntegrationCommand : IntegrationCommand
{
    public int OrderId { get; }
    public Guid OrderNumber { get; }
    public string BuyerName { get; }
    

    public PayOrderIntegrationCommand(int orderId, Guid orderNumber, string buyerName)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        BuyerName = buyerName;
    }
}

