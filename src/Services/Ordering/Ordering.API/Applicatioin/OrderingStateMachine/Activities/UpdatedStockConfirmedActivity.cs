
namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class UpdatedStockConfirmedActivity :
    IStateMachineActivity<OrderState, UpdatedStockConfirmedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;
   
    public UpdatedStockConfirmedActivity(ConsumeContext context, IMediator mediator )
    {
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

    public async Task Execute(BehaviorContext<OrderState, UpdatedStockConfirmedStateIntegrationEvent> context, IBehavior<OrderState, UpdatedStockConfirmedStateIntegrationEvent> next)
    {
        try
        {

            await _mediator.Send(new SetCompletedOrderStatusCommand(context.Message.OrderId));

            await next.Execute(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
        }

    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, UpdatedStockConfirmedStateIntegrationEvent, TException> context,
        IBehavior<OrderState, UpdatedStockConfirmedStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}
