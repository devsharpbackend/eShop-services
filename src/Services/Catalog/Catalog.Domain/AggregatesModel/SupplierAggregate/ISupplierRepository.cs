namespace eShop.Services.Catalog.Domain.AggregatesModel.SupplierAggregate;


public interface ISupplierRepository:IRepository<Supplier>
{
    Supplier Add(Supplier item);
    void Update(Supplier item);
    Task<IEnumerable<Supplier>> Get(int[] id);
    Task<IEnumerable<Supplier>> GetByType(int typeId);
    Task<Supplier> GetById(int id);
    Task<IEnumerable<Supplier>> GetByPaging(int pageSize, int PageIndex);
}