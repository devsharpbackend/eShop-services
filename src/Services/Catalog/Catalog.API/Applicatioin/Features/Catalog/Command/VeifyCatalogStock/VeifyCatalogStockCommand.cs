
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.CreateCatalog.Command;

public class VeifyCatalogStockCommand : IRequest
{
    public int OrderId { get; }
    public Guid OrderNumber { get; }
    public IEnumerable<VeifyCatalogStockItem> OrderStockItems { get; }

    public VeifyCatalogStockCommand(int orderId, Guid orderNumber,
        IEnumerable<VeifyCatalogStockItem> orderStockItems)
    {
        OrderNumber = orderNumber;
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}

public class VeifyCatalogStockItem
{
    public int CatalogId { get; }
    public int Units { get; }

    public VeifyCatalogStockItem(int catalogId, int units)
    {
        CatalogId = catalogId;
        Units = units;
    }
}
