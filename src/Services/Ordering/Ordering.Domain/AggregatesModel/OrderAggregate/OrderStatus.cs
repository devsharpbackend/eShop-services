namespace eShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

public enum OrderStatus
{
    Submitted = 1,
    AwaitingValidation = 2,
    StockConfirmed = 3,
    Paid = 4,
    Cancelled = 5
}