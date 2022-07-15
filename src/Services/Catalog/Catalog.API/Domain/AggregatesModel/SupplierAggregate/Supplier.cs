namespace eShop.Services.CatalogAPI.Domain.AggregatesModel.SupplierAggregate;

public class Supplier : Entity, IAggregateRoot
{
    private string? _supplierName;
    private int _catalogTypeId;

    public Supplier(string? supplierName, int catalogTypeId)
    {
        this._supplierName = supplierName ?? throw new CatalogDomainException("SupplierName is required");

        this._catalogTypeId = catalogTypeId;

        this._supplieItems = new List<SupplierItem>();
    }

   
    public string? SupplierName => _supplierName;
    public int CatalogTypeId => _catalogTypeId;

    public void SetName(string? supplierName)
    {
        this._supplierName = supplierName ?? throw new CatalogDomainException("SupplierName is required");
    }
  
    private readonly List<SupplierItem> _supplieItems;
    public IReadOnlyCollection<SupplierItem> SupplierItems => _supplieItems;

    public void AddSupplierItem(int _supplierId, int _catalogItemId, int _requestedNumber)
    {
        _supplieItems.Add(new SupplierItem(_supplierId, _catalogItemId, _requestedNumber));
    }
}
