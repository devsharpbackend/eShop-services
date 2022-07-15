
using MassTransit;

namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class StateIntegrationEvent : IIntegrationMessage
{
    public StateIntegrationEvent()
    {
        this.Id = Guid.NewGuid();
        this.CreationDate = DateTime.Now;
    }
    [JsonConstructor]
    public StateIntegrationEvent(Guid Id, DateTime CreationDate)
    {
        this.Id = Id;
        this.CreationDate = CreationDate;
    }


    [JsonInclude]
    public Guid Id  { get; }
    public DateTime CreationDate { get;  }
   
}


