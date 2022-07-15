namespace eShop.Services.CatalogAPI.Infrastructure;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, CatalogContext ctx)
    {
        var domainEntities = ctx.ChangeTracker.Entries<Entity>()
            .Where(p => p.Entity.DomainEvents != null && p.Entity.DomainEvents.Any());

        var domainEventList = domainEntities.SelectMany(p => p.Entity.DomainEvents).ToList();

        domainEntities.ToList().ForEach((entity) => {
            entity.Entity.ClearDomainEvents();
        });

        foreach(var domainEvent in domainEventList)
        {
           await mediator.Publish(domainEvent);
        }
    }
}
