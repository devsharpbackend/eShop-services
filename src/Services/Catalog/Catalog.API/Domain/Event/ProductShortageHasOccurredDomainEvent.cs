namespace eShop.Services.CatalogAPI.Domain.Event;

public class ProductShortageHasOccurredDomainEvent:INotification
{
    public ProductShortageHasOccurredDomainEvent(int requiredInventory, CatalogItem catalogItem)
    {
        RequiredInventory = requiredInventory;
        CatalogItem = catalogItem;
        LowofStockDateTime=DateTime.Now;
    }

    public int RequiredInventory { get;private set; }

    public CatalogItem CatalogItem { get;private set; }

    public DateTime LowofStockDateTime { get;private set; }
}
