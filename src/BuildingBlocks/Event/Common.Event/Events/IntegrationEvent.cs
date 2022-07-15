

namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record IntegrationEvent
{
    
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid Id, DateTime CreationDate)
    {
        this.Id = Id;
        this.CreationDate = CreationDate;
    }

    [JsonInclude]
    public Guid Id { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }
}
