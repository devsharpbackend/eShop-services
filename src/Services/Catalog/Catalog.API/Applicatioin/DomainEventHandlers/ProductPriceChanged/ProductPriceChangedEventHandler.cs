
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.DomainEventHandlers.ProductPriceChanged;

public class ProductPriceChangedEventHandler : INotificationHandler<ProductPriceChangedDomainEvent>
{
    private readonly ILoggerFactory _logger;
    //   private readonly IZeroMqPublisher _zeroMqPublisher;

    //private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
    public ProductPriceChangedEventHandler(ILoggerFactory logger/*, IZeroMqPublisher zeroMqPublisher*/
        //, IPublishEndpoint publishEndpoint ,
        ,
        ICatalogIntegrationEventService catalogIntegrationEventService
        )
    {
        _logger = logger??throw new ArgumentNullException(nameof(logger));
        //  _zeroMqPublisher = zeroMqPublisher??throw new ArgumentNullException(nameof(zeroMqPublisher));
        // _publishEndpoint=publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint)); ;
        _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
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
      //  await _publishEndpoint.Publish(new CatalogPriceChangedIntegrationEvent(catalogId, newPrice, oldPrice));


        await _catalogIntegrationEventService.AddAndSaveEventAsync(new CatalogPriceChangedIntegrationEvent(catalogId, newPrice, oldPrice));

        //  return Task.CompletedTask;
    }
}
