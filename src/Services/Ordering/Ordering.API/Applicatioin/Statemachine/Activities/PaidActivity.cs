using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class PaidActivity :
    IStateMachineActivity<OrderState, PaiedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;

    private readonly IRequestClient<UpdateStockForOrderIntegrationCommand> _requestUpdateStockForOrderIntegrationCommand;

    public PaidActivity(ConsumeContext context, IMediator mediator
        , IRequestClient<UpdateStockForOrderIntegrationCommand> requestUpdateStockForOrderIntegrationCommand
        )
    {
        _context = context;
        _mediator = mediator;
        _requestUpdateStockForOrderIntegrationCommand = requestUpdateStockForOrderIntegrationCommand;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-submitted");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, PaiedStateIntegrationEvent> context, IBehavior<OrderState, PaiedStateIntegrationEvent> next)
    {
        try
        {
            await _mediator.Send(new SetPaidOrderStatusCommand(context.Message.OrderId));

            var order = await _mediator.Send(new GetOrderQuery { OrderId = context.Message.OrderId });

            //Send Integration Command for Checkeing Stock

            var orderStockList = order.orderitems
                .Select(orderItem => new UpdateOrderStockItem(orderItem.productId, orderItem.units));

            var updateStockForOrderIntegrationCommand = new UpdateStockForOrderIntegrationCommand(
            order.OrderId, order.OrderNumber, order.status, order.BuyerName, orderStockList);


            var response = await _requestUpdateStockForOrderIntegrationCommand.GetResponse<JsonErrorResponse, SuccessStockUpdateResponse>(updateStockForOrderIntegrationCommand);

            if (response.Is(out Response<JsonErrorResponse> responseA))
            {
                throw new OrderingApplicationException(responseA.Message);
            };

            if (response.Is(out Response<SuccessStockUpdateResponse> responseB))
            {
                await context.Publish(new 
                    UpdatedStockConfirmedStateIntegrationEvent(responseB.Message.OrderId, responseB.Message.OrderNumber));
            };

          
        }
        catch (OrderingApplicationException ex)
        {
            await context.Publish(new
                    UpdatedStockRejectedStateIntegrationEvent(context.Message.OrderId, context.Message.OrderNumber,ex.JsonErrorResponse.Messages.ToString())); 
        }
        finally
        {
            await next.Execute(context).ConfigureAwait(false);
        }
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, PaiedStateIntegrationEvent, TException> context,
        IBehavior<OrderState, PaiedStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}