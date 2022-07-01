namespace eShop.Services.Catalog.Domain.SeedWork;

public interface IRepository<T>  
{
    IUnitOfWork UnitOfWork { get; }
}
