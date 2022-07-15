
namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class CenceledStateIntegrationEvent : StateIntegrationEvent
{ 
    public CenceledStateIntegrationEvent() : base()
    {

    }
    [JsonConstructor]
    public CenceledStateIntegrationEvent(int OrderId, Guid OrderNumber, string Reason)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        this.Reason = Reason;
    }
    [JsonInclude]
    public int OrderId { get; private init; }

    [JsonInclude]
    public string Reason { get; private init; }

    public Guid OrderNumber { get; private init; }

   
}
