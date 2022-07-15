namespace eShop.BuildingBlocks.Event.CommonEvent.Events;

public record CatalogPriceChangedIntegrationEvent : IntegrationEvent
{
  
    public CatalogPriceChangedIntegrationEvent(int CatalogID, decimal NewPrice, decimal OldPrice)
    {
        this.CatalogID = CatalogID;
        this.NewPrice = NewPrice;
        this.OldPrice = OldPrice;
    }

    [JsonInclude]
    public int CatalogID { get; private init; }
    [JsonInclude]
    public decimal NewPrice { get; private init; }
    [JsonInclude]
    public decimal OldPrice { get; private init; }
}
