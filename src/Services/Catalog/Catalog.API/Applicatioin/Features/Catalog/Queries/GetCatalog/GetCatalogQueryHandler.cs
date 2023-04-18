namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalog;

public class GetCatalogQueryHandler : IRequestHandler<GetCatalogQuery,CatalogVM>

{
    private readonly ICatalogRepository _catalogRepository;

    public GetCatalogQueryHandler(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
    }

    public async Task<CatalogVM?> Handle(GetCatalogQuery request, CancellationToken cancellationToken)
    {
        var item = await _catalogRepository.GetByIdAsync(request.CatalogId);

        if (item != null)
            // mapping ... u can use auto mapper or mapster
            return new CatalogVM
            {
                AvailableStock = item.AvailableStock,
                CatalogTypeId = item.CatalogTypeId,
                Description = item.Description,
                Discount = item.Discount,
                IsDiscount = item.IsDiscount,
                MaxStockThreshold = item.MaxStockThreshold,
                Name = item.Name,
                PictureFileName = item.PictureFileName,
                Price = item.Price,
                PriceWithDiscount = item.PriceWithDiscount,
                StockThreshold = item.StockThreshold,
                ID=item.Id
            };

        return null;

    }
}
