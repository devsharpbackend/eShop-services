
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.DomainEventHandlers.ProductPriceChanged;

public class ProductPriceChangedEventHandler : INotificationHandler<ProductPriceChangedDomainEvent>
{
    private readonly ILoggerFactory _logger;

    public ProductPriceChangedEventHandler(ILoggerFactory logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductPriceChangedDomainEvent productShortageHasOccurredDomainEvent, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<ProductPriceChangedDomainEvent>()
            .LogTrace("Catalog with Id: {CatalogId} has been successfully updated to new Price",
                productShortageHasOccurredDomainEvent?.CatalogItem?.Id);

        // Dispatch Integration Event
        return Task.CompletedTask;
    }
}
