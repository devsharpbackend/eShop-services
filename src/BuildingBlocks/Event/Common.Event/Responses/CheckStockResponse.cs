namespace eShop.BuildingBlocks.Event.CommonEvent.Responses;

public class CheckStockResponse
{
    [JsonConstructor]
    public CheckStockResponse(int OrderId, Guid OrderNumber, List<CheckStockResponseStockItem> orderStockItems)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        OrderStockItems = orderStockItems;
    }
    [JsonInclude]
    public int OrderId { get;private  set; }
    [JsonInclude]
    public List<CheckStockResponseStockItem> OrderStockItems { get; private set; }
    [JsonInclude]
    public Guid OrderNumber { get; private init; }

}

public class CheckStockResponseStockItem
{
    public int CatalogId { get; }
    public bool HasStock { get; }

    public CheckStockResponseStockItem(int catalogId, bool hasStock)
    {
        CatalogId = catalogId;
        HasStock = hasStock;
    }
}