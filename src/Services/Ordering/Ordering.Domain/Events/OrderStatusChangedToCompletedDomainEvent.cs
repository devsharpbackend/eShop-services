namespace eShop.Services.Ordering.Domain.Events;

public class OrderStatusChangedToCompletedDomainEvent : INotification
{
    public Order Order { get; }

    public OrderStatusChangedToCompletedDomainEvent(Order order)
    {
        Order = order;
    }
}
