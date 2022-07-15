
using MassTransit;

namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class OrderSubmittedStateIntegrationEvent : StateIntegrationEvent
{
    public OrderSubmittedStateIntegrationEvent():base()
    {
    }
    [JsonConstructor]
    public OrderSubmittedStateIntegrationEvent(int OrderId, Guid OrderNumber)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
    }
    [JsonInclude]
    public int OrderId { get; private init; }

    public Guid OrderNumber { get; private init; }


}
