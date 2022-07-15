namespace eShop.Services.Ordering.OrderingAPI.Application.Statemachine;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get ; set ; }
    public State CurrentState { get;set; }
    public int OrderId { get;set;}
}
