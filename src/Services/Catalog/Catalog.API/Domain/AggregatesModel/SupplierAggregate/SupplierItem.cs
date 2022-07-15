namespace eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate;

public class SupplierItem : Entity
{
    private int _supplierId;
    private int _requestedNumber;
    private int _catalogItemId;

    public SupplierItem()
    {
    }

    public SupplierItem(int supplierId, int catalogItemId, int requestedNumber)
    {
        this._supplierId = supplierId;
        this._requestedNumber = requestedNumber;
        this._catalogItemId = catalogItemId;
    }

    public int SupplierId => _supplierId;
    public int RequestedNumber => _requestedNumber;
    public int CatalogItemId => _catalogItemId;

  

}
