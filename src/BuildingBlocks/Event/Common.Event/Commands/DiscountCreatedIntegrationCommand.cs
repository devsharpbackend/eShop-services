
namespace eShop.BuildingBlocks.Event.CommonEvent.Commands;

public record DiscountCreatedIntegrationCommand: IntegrationCommand
{
  
    public DiscountCreatedIntegrationCommand(int CatalogID, decimal Amount)
    {
        this.CatalogID = CatalogID;
        this.Amount = Amount;
    }

    [JsonInclude]
    public int CatalogID { get; private init; }
    [JsonInclude]
    public decimal Amount { get; private init; }
}
