


namespace eShop.Services.CatalogAPI.Infrastructure;

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