namespace eShop.Services.Catalog.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly CatalogContext _context;
    public SupplierRepository(CatalogContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(_context)); 
    }

    public IUnitOfWork UnitOfWork => _context;

    public Supplier Add(Supplier item)
    {
        return _context.Add(item).Entity;
    }

    public async Task<IEnumerable<Supplier>> Get(int[] ids)
    {
        var items = await _context.Suppliers.Where(ci => ids.Contains(ci.Id)).ToListAsync();

        return items;
    }

    public async Task<Supplier> GetById(int id)
    {
        var supplierItem = await _context
                              .Suppliers
                              //.Include(ci => ci.SupplierItems)
                              .FirstOrDefaultAsync(C => C.Id == id);

        if (supplierItem != null)
        {
            await _context.Entry(supplierItem).Collection(p => p.SupplierItems).LoadAsync();
        }
        return supplierItem;
    }

    public async Task<IEnumerable<Supplier>> GetByPaging(int pageSize, int pageIndex)
    {
      

        var itemsOnPage = await _context.Suppliers
            .OrderBy(c => c.SupplierName)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return itemsOnPage;
    }

    public async Task<IEnumerable<Supplier>> GetByType(int typeId)
    {
        var items = await _context.Suppliers.Where(p=>p.CatalogTypeId == typeId).ToListAsync();
           
         
        return items;
    }

    public void Update(Supplier item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    
}
