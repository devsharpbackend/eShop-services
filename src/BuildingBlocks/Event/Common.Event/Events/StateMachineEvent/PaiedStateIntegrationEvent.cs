
namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class PaiedSuccessStateIntegrationEvent: StateIntegrationEvent
{
    public PaiedSuccessStateIntegrationEvent() : base()
    {


    }

    [JsonConstructor]
    public PaiedSuccessStateIntegrationEvent(int OrderId, Guid OrderNumber,string TransactionId)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        this.TransactionId = TransactionId;
    }
    [JsonInclude]
    public int OrderId { get; private init; }
    [JsonInclude]
    public Guid OrderNumber { get; private init; }
    [JsonInclude]
    public string TransactionId { get; private init; }


}

