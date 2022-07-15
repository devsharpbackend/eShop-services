

namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record IntegrationEvent: IntegrationMessage
{
    public IntegrationEvent() : base()
    {
    }

    [JsonConstructor]
    public IntegrationEvent(Guid Id, DateTime CreationDate) : base(Id, CreationDate)
    {
    }
}
