namespace eShop.Services.CatalogAPI.Application.IntegrationMessages;
public interface ICatalogIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IIntegrationMessage evt);
    Task PublishUnSendedEventsThroughEventBusAsync();
}
