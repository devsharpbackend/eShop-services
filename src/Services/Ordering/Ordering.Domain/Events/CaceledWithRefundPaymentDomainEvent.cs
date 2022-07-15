namespace eShop.Services.Ordering.Domain.Events;

public class CaceledWithRefundPaymentDomainEvent : INotification
{
    public Order Order { get; }

    public CaceledWithRefundPaymentDomainEvent(Order order)
    {
        Order = order;
    }
}

