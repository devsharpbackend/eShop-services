namespace eShop.Services.CatalogAPI.Infrastructure;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, CatalogContext ctx)
    {
      
        var domainEntities=ctx.ChangeTracker.Entries<Entity>()
            .Where(p=>p.Entity.DomainEvents!=null && p.Entity.DomainEvents.Any()).ToList();

        
        var domainEvents= domainEntities.SelectMany(p=>p.Entity.DomainEvents).ToList();

        // clear Domain Events
        domainEntities.ForEach(e =>e.Entity.ClearDomainEvents());   

        // publish events
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }

    }
}
