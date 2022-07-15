using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;

public class StockRejectedActivity :
    IStateMachineActivity<OrderState, StockRejectedStateIntegrationEvent>
{
    readonly ConsumeContext _context;
    private readonly IMediator _mediator;
   
    public StockRejectedActivity(ConsumeContext context, IMediator mediator )
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

    public async Task Execute(BehaviorContext<OrderState, StockRejectedStateIntegrationEvent> context, IBehavior<OrderState, StockRejectedStateIntegrationEvent> next)
    {
        try
        {

            await _mediator.Send(new SetStockRejectedOrderStatusCommand(context.Message.OrderId, context.Message.OrderStockItems.Where(p=>!p.HasStock).Select(p => p.CatalogId).ToList()));
            await next.Execute(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
          
        }

    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, StockRejectedStateIntegrationEvent, TException> context,
        IBehavior<OrderState, StockRejectedStateIntegrationEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}
