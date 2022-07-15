namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class PayFailedActivity :
    IStateMachineActivity<OrderState, PayFaieldStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;
   

    public PayFailedActivity(ConsumeContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-submitted");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, PayFaieldStateIntegrationEvent> context, IBehavior<OrderState, PayFaieldStateIntegrationEvent> next)
    {
        try
        {
            
            // Update Database
            await _mediator.Send(new CancelOrderCommand(context.Message.OrderId));

            await next.Execute(context).ConfigureAwait(false);
        }
        catch (OrderingApplicationException ex)
        {
          
        }
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, PayFaieldStateIntegrationEvent, TException> context,
        IBehavior<OrderState, PayFaieldStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}