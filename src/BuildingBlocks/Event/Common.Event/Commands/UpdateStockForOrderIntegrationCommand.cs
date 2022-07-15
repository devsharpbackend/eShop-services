namespace eShop.BuildingBlocks.Event.CommonEvent.Commands;

public record UpdateStockForOrderIntegrationCommand : IntegrationCommand
{
    public int OrderId { get; }
    public Guid OrderNumber { get; }
    public int OrderStatus { get; }
    public string BuyerName { get; }
    public IEnumerable<UpdateOrderStockItem> OrderStockItems { get; }

    public UpdateStockForOrderIntegrationCommand(int orderId, Guid orderNumber, int orderStatus, string buyerName,
        IEnumerable<UpdateOrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OrderStockItems = orderStockItems;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }
}

public record UpdateOrderStockItem
{
    public int CatalogId { get; }
    public int Units { get; }

    public UpdateOrderStockItem(int catalogId, int units)
    {
        CatalogId = catalogId;
        Units = units;
    }
}
