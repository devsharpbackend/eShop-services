using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class UpdatedStockRejectedActivity :
    IStateMachineActivity<OrderState, UpdatedStockRejectedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;
   
    public UpdatedStockRejectedActivity(ConsumeContext context, IMediator mediator )
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

    public async Task Execute(BehaviorContext<OrderState, UpdatedStockRejectedStateIntegrationEvent> context, IBehavior<OrderState, UpdatedStockRejectedStateIntegrationEvent> next)
    {
        try
        {

            await _mediator.Send(new SetPayRejectedOrderStatusCommand(context.Message.OrderId));

            await next.Execute(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
        }

    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, UpdatedStockRejectedStateIntegrationEvent, TException> context,
        IBehavior<OrderState, UpdatedStockRejectedStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}
