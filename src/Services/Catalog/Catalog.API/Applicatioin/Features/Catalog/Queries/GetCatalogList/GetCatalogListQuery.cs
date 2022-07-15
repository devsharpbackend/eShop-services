namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalogList;

public class GetCatalogListQuery:IRequest<CatalogListVM>
{
    public int PageIndex { get; set; }
    public int PageCount { get; set; }
}