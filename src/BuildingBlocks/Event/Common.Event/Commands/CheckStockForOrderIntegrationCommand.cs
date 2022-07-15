namespace eShop.BuildingBlocks.Event.CommonEvent.Commands;

public record CheckStockForOrderIntegrationCommand : IntegrationCommand
{
    public int OrderId { get; }
    public Guid OrderNumber { get; }
    public int OrderStatus { get; }
    public string BuyerName { get; }
    public IEnumerable<CheckOrderStockItem> OrderStockItems { get; }

    public CheckStockForOrderIntegrationCommand(int orderId, Guid orderNumber, int orderStatus, string buyerName,
        IEnumerable<CheckOrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OrderStockItems = orderStockItems;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}

public record CheckOrderStockItem
{
    public int CatalogId { get; }
    public int Units { get; }

    public CheckOrderStockItem(int catalogId, int units)
    {
        CatalogId = catalogId;
        Units = units;
    }
}
