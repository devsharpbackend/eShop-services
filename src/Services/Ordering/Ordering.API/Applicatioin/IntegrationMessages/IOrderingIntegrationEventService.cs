
namespace eShop.Services.Ordering.OrderingAPI.Application.IntegrationEvents;
public interface IOrderingIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IIntegrationMessage evt);
    Task PublishUnSendedEventsThroughEventBusAsync();
}
