namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalog;

public class GetCatalogQuery:IRequest<CatalogVM>
{
    public int CatalogId { get; set; }
}
