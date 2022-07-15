
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Command.UpdateDiscount;


public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand>, ITransactionBehaviour
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly ILogger<UpdateDiscountCommandHandler> _logger;

    public UpdateDiscountCommandHandler(ICatalogRepository catalogRepository, ILogger<UpdateDiscountCommandHandler> logger)
    {
        _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

      
    }

    public async Task<Unit> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var catalogItem = await _catalogRepository.GetByIdAsync(request.Id);

        if (catalogItem == null)
        {
            throw new NotFoundException(nameof(catalogItem), request.Id);
        }

      
        if (catalogItem != null)
        {
            catalogItem.SetDiscount(request.Amount);
            await _catalogRepository.UnitOfWork.SaveChangesAsync();
        }
        await _catalogRepository.UnitOfWork.SaveEntitiesAsync();

        _logger.LogInformation("-----  Catalog Price Update - Catalog: {@Catalog}", catalogItem);


        return Unit.Value;
    }
}
