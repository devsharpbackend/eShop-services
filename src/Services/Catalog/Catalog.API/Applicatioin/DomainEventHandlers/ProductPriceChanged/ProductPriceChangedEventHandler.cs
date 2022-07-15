
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.DomainEventHandlers.ProductPriceChanged;

public class ProductPriceChangedEventHandler : INotificationHandler<ProductPriceChangedDomainEvent>
{
    private readonly ILoggerFactory _logger;
    //   private readonly IZeroMqPublisher _zeroMqPublisher;
   

    public ProductPriceChangedEventHandler(ILoggerFactory logger/*, IZeroMqPublisher zeroMqPublisher*/)
    {
        _logger = logger??throw new ArgumentNullException(nameof(logger));
      //  _zeroMqPublisher = zeroMqPublisher??throw new ArgumentNullException(nameof(zeroMqPublisher));
   
    }

    public async Task Handle(ProductPriceChangedDomainEvent productShortageHasOccurredDomainEvent, CancellationToken cancellationToken)
    {
        _logger.CreateLogger<ProductPriceChangedDomainEvent>()
            .LogTrace("Catalog with Id: {CatalogId} has been successfully updated to new Price",
                productShortageHasOccurredDomainEvent?.CatalogItem?.Id);


        decimal oldPrice = productShortageHasOccurredDomainEvent.OldPrice;
        decimal newPrice = productShortageHasOccurredDomainEvent.NewPrice;
        int catalogId = productShortageHasOccurredDomainEvent.CatalogItem.Id;
        // Dispatch Integration Event

        // _zeroMqPublisher.Publish(new CatalogPriceChangedIntegrationEvent(catalogId,newPrice,oldPrice));
      

      //  return Task.CompletedTask;
    }
}
