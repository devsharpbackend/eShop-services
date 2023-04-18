
namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class UpdatedStockConfirmedStateIntegrationEvent : StateIntegrationEvent
{
    public UpdatedStockConfirmedStateIntegrationEvent() : base()
    {

    }
    [JsonConstructor]
    public UpdatedStockConfirmedStateIntegrationEvent(int OrderId, Guid OrderNumber)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
    }
    [JsonInclude]
    public int OrderId { get; private init; }

    public Guid OrderNumber { get; private init; }
}
