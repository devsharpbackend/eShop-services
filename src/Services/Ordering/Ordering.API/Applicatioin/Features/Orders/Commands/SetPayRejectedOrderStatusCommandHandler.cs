namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;

// Regular CommandHandler
public class SetPayRejectedOrderStatusCommandHandler : IRequestHandler<SetPayRejectedOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;

    public SetPayRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when
    /// Shipment service confirms the payment
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<Unit> Handle(SetPayRejectedOrderStatusCommand command, CancellationToken cancellationToken)
    {
     
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderId);
        if (orderToUpdate == null)
        {
            throw new NotFoundException(nameof(orderToUpdate), command.OrderId);
        }

        orderToUpdate.SetCaceledWithRefundPayment();
        
        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;

    }
}