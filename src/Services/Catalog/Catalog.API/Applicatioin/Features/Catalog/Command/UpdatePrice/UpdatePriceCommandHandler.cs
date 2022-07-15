
namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Command.UpdatePrice;


public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand>, ITransactionBehaviour
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly ILogger<UpdatePriceCommandHandler> _logger;

    public UpdatePriceCommandHandler(ICatalogRepository catalogRepository, ILogger<UpdatePriceCommandHandler> logger)
    {
        _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

      
    }

    public async Task<Unit> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
    {
        var catalogItem = await _catalogRepository.GetByIdAsync(request.Id);

        if (catalogItem == null)
        {
            throw new NotFoundException(nameof(catalogItem), request.Id);
        }

        catalogItem.UpdateName(request.Name);
        catalogItem.UpdatePrice(request.Price);

        

        await _catalogRepository.UnitOfWork.SaveEntitiesAsync();

        _logger.LogInformation("-----  Catalog Price Update - Catalog: {@Catalog}", catalogItem);


        return Unit.Value;
    }
}
