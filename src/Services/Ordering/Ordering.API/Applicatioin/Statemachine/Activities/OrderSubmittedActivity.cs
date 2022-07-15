using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class OrderSubmittedActivity :
    IStateMachineActivity<OrderState, OrderSubmittedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;
    private readonly IRequestClient<CheckStockForOrderIntegrationCommand> _requestCheckStockForOrderIntegrationCommand;

    public OrderSubmittedActivity(ConsumeContext context, IMediator mediator
       , IRequestClient<CheckStockForOrderIntegrationCommand> requestCheckStockForOrderIntegrationCommand)
    {
        _context = context;
        _mediator = mediator;
        _requestCheckStockForOrderIntegrationCommand = requestCheckStockForOrderIntegrationCommand ?? throw new ArgumentNullException(nameof(requestCheckStockForOrderIntegrationCommand));

    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("order-submitted");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, OrderSubmittedStateIntegrationEvent> context, IBehavior<OrderState, OrderSubmittedStateIntegrationEvent> next)
    { 
        try
        {
            var order = await _mediator.Send(new GetOrderQuery { OrderId = context.Message.OrderId });
            if (order.status != (int)OrderStatus.AwaitingValidation)
                await _mediator.Send(new SetAwaitingValidationOrderStatusCommand { OrderId = context.Message.OrderId });

            //Send Integration Command for Checkeing Stock

            var orderStockList = order.orderitems
                .Select(orderItem => new CheckOrderStockItem(orderItem.productId, orderItem.units));

            var checkStockForOrderIntegrationCommand = new CheckStockForOrderIntegrationCommand(
              order.OrderId, order.OrderNumber, order.status, order.BuyerName, orderStockList);

            var response = await _requestCheckStockForOrderIntegrationCommand.GetResponse<JsonErrorResponse, CheckStockResponse>(checkStockForOrderIntegrationCommand);

            if (response.Is(out Response<JsonErrorResponse> responseA))
            {
                throw new OrderingApplicationException(responseA.Message);
            };

            if (response.Is(out Response<CheckStockResponse> responseB))
            {
                if (responseB.Message.OrderStockItems.Any(c => !c.HasStock))
                {
                    List<ConfirmedOrderStockItem> OrderStockItems = new List<ConfirmedOrderStockItem>();
                    foreach (var item in responseB.Message.OrderStockItems)
                    {
                        OrderStockItems.Add(new ConfirmedOrderStockItem(item.CatalogId, item.HasStock));
                    }
                    await context.Publish(new StockRejectedStateIntegrationEvent(order.OrderId, order.OrderNumber, OrderStockItems));
                }
                else
                {
                    await context.Publish(new StockConfirmedStateIntegrationEvent(order.OrderId, order.OrderNumber));
                }
            };
        }
        catch (OrderingApplicationException ex)
        {
            await context.TransitionToState(context.StateMachine.Initial);
        }
        finally
        {
            await next.Execute(context).ConfigureAwait(false);
        }
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderSubmittedStateIntegrationEvent, TException> context,
        IBehavior<OrderState, OrderSubmittedStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}