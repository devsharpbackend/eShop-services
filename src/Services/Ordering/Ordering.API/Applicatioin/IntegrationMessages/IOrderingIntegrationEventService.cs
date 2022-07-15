namespace eShop.Services.Ordering.OrderingAPI.Application.IntegrationMessages;
public interface IOrderingIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent evt);
    Task PublishUnSendedEventsThroughEventBusAsync();
}
