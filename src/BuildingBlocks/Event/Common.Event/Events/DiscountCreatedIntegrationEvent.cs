
namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record DiscountCreatedIntegrationEvent:IntegrationEvent
{
  
    public DiscountCreatedIntegrationEvent(int CatalogID, decimal Amount)
    {
        this.CatalogID = CatalogID;
        this.Amount = Amount;
    }

    [JsonInclude]
    public int CatalogID { get; private init; }
    [JsonInclude]
    public decimal Amount { get; private init; }
}
