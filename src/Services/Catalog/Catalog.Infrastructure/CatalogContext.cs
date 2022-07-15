



using System.Data;

namespace eShop.Services.Catalog.Infrastructure;

public class CatalogContext : DbContext,IUnitOfWork
{
  private readonly IMediator _mediator;
    public CatalogContext(DbContextOptions<CatalogContext> options,IMediator mediator) : base(options)
    {
            _mediator = mediator;
    }
    public DbSet<CatalogItem> CatalogItems { get; set; }
   
    public DbSet<CatalogType> CatalogTypes { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
      

        // Dispatch Domain Events
        await _mediator.DispatchDomainEventsAsync(this);

        int k=await this.SaveChangesAsync(cancellationToken);
        return true;
    }






    /// <summary>
    ///  TRansaction
    /// </summary>
    private IDbContextTransaction _currentTransaction;

    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction!= null?_currentTransaction:Database.CurrentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction { transaction.TransactionId } is not current");

        try
        {
            await SaveChangesAsync();
            transaction.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        builder.ApplyConfiguration(new SupplierEntityTypeConfiguration());

        builder.ApplyConfiguration(new SupplierItemEntityTypeConfiguration());

    }
}


public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
            .UseSqlServer("Server=.;Initial Catalog=CatalogDb;Integrated Security=true");

        return new CatalogContext(optionsBuilder.Options,new NoMediator());
    }
}


class NoMediator : IMediator
{
    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return null;
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
    {
        return null;
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
       return Task.CompletedTask;
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        return Task.CompletedTask;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return (Task<TResponse>)Task.CompletedTask;
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        return (Task<object?>)Task.CompletedTask;
    }
}