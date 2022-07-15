
namespace eShop.BuildingBlocks.Event.CommonEvent.Commands;

public record IntegrationCommand:IntegrationMessage
{
    public IntegrationCommand():base()
    {
    }

    [JsonConstructor]
    public IntegrationCommand(Guid Id, DateTime CreationDate):base(Id,CreationDate)
    {
    }
}
