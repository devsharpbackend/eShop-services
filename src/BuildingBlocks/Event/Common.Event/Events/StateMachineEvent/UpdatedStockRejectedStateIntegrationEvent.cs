
namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class UpdatedStockRejectedStateIntegrationEvent : StateIntegrationEvent
{
    public UpdatedStockRejectedStateIntegrationEvent() : base()
    {

    }
    [JsonConstructor]
    public UpdatedStockRejectedStateIntegrationEvent(int OrderId, Guid OrderNumber, string Reason)
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
    public string Reason { get;private set; }
}
