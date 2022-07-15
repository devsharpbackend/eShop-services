namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.CreateCatalog.Command;


public class VeifyCatalogStockCommandHandler : IRequestHandler<VeifyCatalogStockCommand>
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly ILogger<CreateCatalogCommandHandler> _logger;
    private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;

    public VeifyCatalogStockCommandHandler(ICatalogRepository catalogRepository
       , ICatalogIntegrationEventService catalogIntegrationEventService
        , ILogger<CreateCatalogCommandHandler> logger)
    {
        _catalogRepository = catalogRepository??throw new ArgumentNullException(nameof(catalogRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _catalogIntegrationEventService= catalogIntegrationEventService??throw new ArgumentNullException(nameof(catalogIntegrationEventService));
    }

    public async Task<Unit> Handle(VeifyCatalogStockCommand request, CancellationToken cancellationToken)
    {
        var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

        foreach (var orderStockItem in request.OrderStockItems)
        {
            var catalogItem =await _catalogRepository.GetByIdAsync(orderStockItem.CatalogId);
            var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
            var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);
            confirmedOrderStockItems.Add(confirmedOrderStockItem);
        }

        IIntegrationMessage confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? new StockRejectedStateIntegrationEvent(request.OrderId,request.OrderNumber, confirmedOrderStockItems)
            : new StockConfirmedStateIntegrationEvent(request.OrderId,request.OrderNumber);

        await _catalogIntegrationEventService.AddAndSaveEventAsync(confirmedIntegrationEvent);

        return Unit.Value;
    }

    
}
