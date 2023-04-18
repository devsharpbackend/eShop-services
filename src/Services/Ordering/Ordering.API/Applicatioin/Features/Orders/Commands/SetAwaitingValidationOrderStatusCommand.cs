namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;


public class SetAwaitingValidationOrderStatusCommand : IRequest<bool>
{
    public int OrderId { get;  set; }

}
