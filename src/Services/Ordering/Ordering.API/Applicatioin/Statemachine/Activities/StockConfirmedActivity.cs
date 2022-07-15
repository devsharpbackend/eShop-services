using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class StockConfirmedActivity :
    IStateMachineActivity<OrderState, StockConfirmedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;
    private readonly IRequestClient<PayOrderIntegrationCommand> _requestPayOrderIntegrationCommand;

    public StockConfirmedActivity(ConsumeContext context, IMediator mediator
        , IRequestClient<PayOrderIntegrationCommand> requestPayOrderIntegrationCommand)
    {
        _requestPayOrderIntegrationCommand = requestPayOrderIntegrationCommand ?? throw new ArgumentNullException(nameof(requestPayOrderIntegrationCommand));
        _context = context;
        _mediator = mediator;
    }

    public void Probe(ProbeContext context)
    {

    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, StockConfirmedStateIntegrationEvent> context, IBehavior<OrderState, StockConfirmedStateIntegrationEvent> next)
    {
        try
        {
            // update Data base
            await _mediator.Send(new SetStockConfirmedOrderStatusCommand(context.Message.OrderId));

            var order = await _mediator.Send(new GetOrderQuery { OrderId = context.Message.OrderId });
            if (order != null)
            {
                var payOrderIntegrationCommand = new PayOrderIntegrationCommand(order.OrderId, order.OrderNumber, order.BuyerName);

                var response = await _requestPayOrderIntegrationCommand.GetResponse<JsonErrorResponse, CheckPaidResponse>(payOrderIntegrationCommand);

                if (response.Is(out Response<JsonErrorResponse> responseA))
                {
                    throw new OrderingApplicationException(responseA.Message);
                };

                if (response.Is(out Response<CheckPaidResponse> responsePaied))
                {
                    if (responsePaied.Message.IsPaid)
                    {
                        PaiedStateIntegrationEvent paiedStateIntegration = new PaiedStateIntegrationEvent(responsePaied.Message.OrderId, responsePaied.Message.OrderNumber, responsePaied.Message.TransactionId);
                        await context.Publish(paiedStateIntegration);
                    }
                    else
                    {
                        PayFaieldStateIntegrationEvent paiedStateIntegration = new PayFaieldStateIntegrationEvent(responsePaied.Message.OrderId, responsePaied.Message.OrderNumber, responsePaied.Message.Reason);
                        await context.Publish(paiedStateIntegration);
                    }
                };
            }
        }
        catch (Exception ex)
        {
            await context.Publish(new PayFaieldStateIntegrationEvent(context.Message.OrderId, context.Message.OrderNumber,ex.Message));
        }
        finally
        {
            await next.Execute(context).ConfigureAwait(false);
        }
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, StockConfirmedStateIntegrationEvent, TException> context,
        IBehavior<OrderState, StockConfirmedStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}
