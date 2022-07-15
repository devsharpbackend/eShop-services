using eShop.BuildingBlocks.Event.CommonEvent.StateMachineEvent;

namespace eShop.Services.Ordering.OrderingAPI.Application.BackgroundServices;

public class OrderSubmittedPublisherWorker : BackgroundService
{

    private readonly IServiceScopeFactory _serviceScopeFactory;
    public OrderSubmittedPublisherWorker(IServiceScopeFactory serviceScopeFactory)
    {
       _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var _logger =scope.ServiceProvider.GetRequiredService<ILogger<EventPublinsherWorker>>();
       
        var _options=scope.ServiceProvider.GetRequiredService<IOptions<OrderSetting>>();
        var _mediator= scope.ServiceProvider.GetRequiredService<IMediator>();
        var _publishEndpoint= scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
    _logger.LogDebug("OrderSubmittedPublisherWorker is starting.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("OrderSubmittedPublisherWorker background task is doing background work.");

            var orderListId =await _mediator.Send(new GetOrdersByStatusQuery {  orderStatus=OrderStatus.Submitted});

            foreach (var order in orderListId)
            {
                await _publishEndpoint.Publish(new OrderSubmittedStateIntegrationEvent(order.OrderId,order.OrderNumber));
            }
            await Task.Delay(_options.Value.CheckUpdateTimeForEventPublinsherWorker, stoppingToken);
        }
     
        _logger.LogDebug("OrderSubmittedPublisherWorker background task is stopping.");
        stoppingToken.Register(() => _logger.LogDebug("#1 OrderSubmittedPublisherWorker background task is stopping."));
    }

}