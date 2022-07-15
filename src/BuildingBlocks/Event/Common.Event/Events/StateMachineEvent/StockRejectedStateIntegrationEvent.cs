
namespace eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;


public class StockRejectedStateIntegrationEvent : StateIntegrationEvent
{
    public StockRejectedStateIntegrationEvent() : base()
    {


    }
    [JsonConstructor]
    public StockRejectedStateIntegrationEvent(int OrderId, Guid OrderNumber, List<ConfirmedOrderStockItem> orderStockItems)
    {
        this.OrderId = OrderId;
        this.OrderNumber = OrderNumber;
        OrderStockItems = orderStockItems;
    }
    [JsonInclude]
    public int OrderId { get; private init; }
    public string Reason { get;private init; }
    [JsonInclude]
    public List<ConfirmedOrderStockItem> OrderStockItems { get; }
    [JsonInclude]
    public Guid OrderNumber { get; private init; }

}


public record ConfirmedOrderStockItem
{
    public int CatalogId { get; }
    public bool HasStock { get; }

    public ConfirmedOrderStockItem(int catalogId, bool hasStock)
    {
        CatalogId = catalogId;
        HasStock = hasStock;
    }
}