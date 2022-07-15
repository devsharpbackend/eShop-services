using eShop.Services.Ordering.OrderingAPI.Applicatioin.Statemachine.Activities;


namespace eShop.Services.Ordering.OrderingAPI.Application.Statemachine;

public class OrderStateMachine :
    MassTransitStateMachine<OrderState>
{

    public OrderStateMachine(
        ILogger<OrderStateMachine> logger)
    {
        
        this.InstanceState(x => x.CurrentState);
        this.ConfigureCorrelationIds();


        Initially(
              When(OrderSubmittedEvent)
              .Then(x => { x.Instance.Created = DateTime.Now;x.Instance.OrderId = x.Message.OrderId; })
              .Then((x) =>
              {
                  logger.LogInformation($" Order With Number {x.Message.OrderNumber} has been Submitted");
              })
              .Activity(x => x.OfType<OrderSubmittedActivity>())
              .TransitionTo(Submitted));



        During(Submitted,
             When(StockIsConfirmedEvent)
             .Then(x => logger.LogInformation($"OrderStateMachine Order {x.Instance.OrderId} StockIsConfirmed"))
             // .Activity(x => x.OfType<StockConfirmedActivity>())
             .TransitionTo(StockConfirmed),
             When(StockIsRejectedEvent)
             .Then(x => logger.LogInformation($"OrderStateMachine Order {x.Instance.OrderId} rejected! because {x.Data.Reason}"))
             //      .Activity(x => x.OfType<StockRejectedActivity>())
             .TransitionTo(StockRejected).Finalize());



        During(StockConfirmed,
               When(PayFaieldEvent)
                 .Then(x => logger.LogInformation($"OrderStateMachine Order {x.Instance.OrderId} pay is faield!"))
                 //  .Activity(x => x.OfType<PayFailedActivity>())
                 .TransitionTo(PayFaield)
                 .Finalize(),
               When(PaiedSuccessEvent)
                .Then(x => logger.LogInformation($"OrderStateMachine Order {x.Instance.OrderId} pay is paied!"))
                //     .Activity(x => x.OfType<PaidActivity>())
                .TransitionTo(Paied));


        During(Paied,
             When(UpdatedStockConfirmedEvent)
                 .Then(x => logger.LogInformation($"OrderStateMachine Order {x.Instance.OrderId} pay is UpdatedStockConfirmed!"))
                 //  .Activity(x => x.OfType<UpdatedStockConfirmedActivity>())
                 .Finalize(),
            When(UpdatedStockRejectedEvent)
                 .Then(x => logger.LogInformation($"OrderStateMachine Order {x.Instance.OrderId} pay is UpdatedStockRejected!"))
                 //    .Activity(x => x.OfType<UpdatedStockRejectedActivity>())
               //  .TransitionTo(Canceled)
                 .Finalize());



        DuringAny(
            When(CanceledEvent)
            .Then(x => logger.LogInformation($"OrderStateMachine1 Order {x.Instance.OrderId} Canceled! because {x.Data.Reason}"))
            .TransitionTo(Canceled)
            .Finalize());

        During(Canceled,
           Ignore(OrderSubmittedEvent));

        



    }



    private void ConfigureCorrelationIds()
    {
       //  Map OrderNumber To CorrelationId In Instance
        Event(() => OrderSubmittedEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => StockIsConfirmedEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => StockIsRejectedEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => StockIsRejectedEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => PaiedSuccessEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => PayFaieldEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => UpdatedStockConfirmedEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => UpdatedStockRejectedEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
        Event(() => CanceledEvent, x => x.CorrelateById(x => x.Message.OrderNumber));
    }


    public State Submitted { get; private set; }
    public State StockConfirmed { get; private set; }
    public State StockRejected { get; private set; }
    public State Paied { get; private set; }
    public State PayFaield { get; private set; }
    public State Canceled { get; private set; }



    public Event<OrderSubmittedStateIntegrationEvent> OrderSubmittedEvent { get; private set; }
    public Event<StockConfirmedStateIntegrationEvent> StockIsConfirmedEvent { get; private set; }
    public Event<StockRejectedStateIntegrationEvent> StockIsRejectedEvent { get; private set; }
    public Event<PaiedSuccessStateIntegrationEvent> PaiedSuccessEvent { get; private set; }
    public Event<PayFaieldStateIntegrationEvent> PayFaieldEvent { get; private set; }
    public Event<UpdatedStockConfirmedStateIntegrationEvent> UpdatedStockConfirmedEvent { get; private set; }
    public Event<UpdatedStockRejectedStateIntegrationEvent> UpdatedStockRejectedEvent { get; private set; }
    public Event<CenceledStateIntegrationEvent> CanceledEvent { get; private set; }

}
