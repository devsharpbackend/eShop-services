
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Commands.UpdateStock;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, int>
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly ILogger<UpdateStockCommandHandler> _logger;

    public UpdateStockCommandHandler(ICatalogRepository catalogRepository, ILogger<UpdateStockCommandHandler> logger)
    {
        _catalogRepository = catalogRepository?? throw new ArgumentNullException(nameof(catalogRepository));
        _logger = logger?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var catalogItem = await _catalogRepository.GetByIdAsync(request.Id);

        if (catalogItem == null)
        {
            throw new NotFoundException(nameof(CatalogItem), request.Id);
        }

       int removed= catalogItem.RemoveStock(request.Quantity);

        await _catalogRepository.UnitOfWork.SaveEntitiesAsync();

        _logger.LogInformation("----- Stock Catalog Update - Catalog: {@Catalog}", catalogItem);

        return removed;
    }
}
