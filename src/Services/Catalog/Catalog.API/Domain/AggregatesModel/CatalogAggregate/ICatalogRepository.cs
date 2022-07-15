namespace eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogAggregate;


public interface ICatalogRepository : IRepository<CatalogItem> 
{
    CatalogItem Add(CatalogItem item);
    void Update(CatalogItem item);
    Task<IEnumerable<CatalogItem>> Get(int[] ids);
   Task<CatalogItem> GetByIdAsync(int id);
    Task<IEnumerable<CatalogItem>> GetByPagingAsync(int pageSize,int PageIndex);

}
