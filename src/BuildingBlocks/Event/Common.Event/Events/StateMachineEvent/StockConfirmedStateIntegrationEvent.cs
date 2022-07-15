
namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class StockConfirmedStateIntegrationEvent : StateIntegrationEvent
{
    public StockConfirmedStateIntegrationEvent() : base()
    {

    }
    [JsonConstructor]
    public StockConfirmedStateIntegrationEvent(int OrderId, Guid OrderNumber)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
    }
    [JsonInclude]
    public int OrderId { get; private init; }

    public Guid OrderNumber { get; private init; }
}
