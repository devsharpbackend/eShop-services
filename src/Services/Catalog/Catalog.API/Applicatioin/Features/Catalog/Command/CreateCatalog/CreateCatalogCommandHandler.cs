namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.CreateCatalog.Command;


public class CreateCatalogCommandHandler : IRequestHandler<CreateCatalogCommand, int>, ITransactionBehaviour
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly ILogger<CreateCatalogCommandHandler> _logger;

    public CreateCatalogCommandHandler(ICatalogRepository catalogRepository, ILogger<CreateCatalogCommandHandler> logger)
    {
        _catalogRepository = catalogRepository??throw new ArgumentNullException(nameof(catalogRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> Handle(CreateCatalogCommand request, CancellationToken cancellationToken)
    {

        CatalogItem catalogItem = new CatalogItem(name: request.Name, description: request.Description, price: request.Price, isDiscount: request.IsDiscount, pictureFileName: request.PictureFileName
             , catalogTypeId: request.CatalogTypeId, availableStock: request.AvailableStock, stockThreshold: request.StockThreshold, maxStockThreshold: request.MaxStockThreshold);
            

        _catalogRepository.Add(catalogItem);

        await _catalogRepository.UnitOfWork.SaveChangesAsync();
        _logger.LogInformation("-----  Catalog Create - Catalog: {@Catalog}", catalogItem);

        return catalogItem.Id;
    }
}
