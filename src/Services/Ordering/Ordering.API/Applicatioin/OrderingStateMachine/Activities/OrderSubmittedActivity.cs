

namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class OrderSubmittedActivity :
    IStateMachineActivity<OrderState, OrderSubmittedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;


    public OrderSubmittedActivity(ConsumeContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;

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


        }
        catch (Exception ex)
        {

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
