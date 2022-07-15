namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands;


public class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Handler which processes the command when
    /// Stock service rejects the request
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // Simulate a work time for rejecting the stock
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(command.OrderId);
        if (orderToUpdate == null)
        {
            throw new NotFoundException(nameof(orderToUpdate), command.OrderId);
        }

        orderToUpdate.SetCancelledStatusWhenStockIsRejected(command.OrderStockItems);
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
