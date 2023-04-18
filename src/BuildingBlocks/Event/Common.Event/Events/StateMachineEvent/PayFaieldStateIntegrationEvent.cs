namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;

public class PayFaieldStateIntegrationEvent : StateIntegrationEvent
{
    public PayFaieldStateIntegrationEvent() : base()
    {
    }
    [JsonConstructor]
    public PayFaieldStateIntegrationEvent(int OrderId, Guid OrderNumber,string Reason)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        this.Reason = Reason;
    }
    [JsonInclude]
    public int OrderId { get; private init; }
    [JsonInclude]
    public Guid OrderNumber { get; private init; }
    [JsonInclude]
    public string Reason { get; private set; }
}
