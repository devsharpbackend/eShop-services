namespace eShop.Services.Catalog.CatalogAPI.Application.DomainEventHandlers.ProductShortageHasOccurred;

public class ProductShortageHasOccurredEventHandler : INotificationHandler<ProductShortageHasOccurredDomainEvent>
{
    private readonly ILoggerFactory _logger;
    private readonly ISupplierRepository _supplierRepository;

    public ProductShortageHasOccurredEventHandler(ILoggerFactory logger, ISupplierRepository supplierRepository)
    {
        _logger = logger;
        _supplierRepository = supplierRepository?? throw new ArgumentNullException(nameof(ISupplierRepository));
    }

    public async Task Handle(ProductShortageHasOccurredDomainEvent productShortageHasOccurredDomainEvent, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<ProductPriceChangedDomainEvent>()
              .LogTrace("Catalog with Id: {CatalogId} has been successfully updated to new Quantity Stock",
                  productShortageHasOccurredDomainEvent.CatalogItem?.Id);


        var suppliersList = await _supplierRepository.GetByType(productShortageHasOccurredDomainEvent.CatalogItem.CatalogTypeId);


        foreach(var supplier in suppliersList)
        {
            supplier.AddSupplierItem(supplier.Id, productShortageHasOccurredDomainEvent.CatalogItem.Id, productShortageHasOccurredDomainEvent.RequiredInventory);

            _supplierRepository.Update(supplier);
        }

       await _supplierRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
