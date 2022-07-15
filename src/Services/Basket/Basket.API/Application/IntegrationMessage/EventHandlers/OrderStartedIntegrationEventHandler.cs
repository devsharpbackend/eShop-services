namespace eShop.Services.Basket.BasketAPI.Application.IntegrationMessage.EventHandlers;


public class OrderStartedIntegrationEventHandler : IConsumer<OrderStartedIntegrationEvent>
{

    private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

    private readonly IMediator _mediator;

    public OrderStartedIntegrationEventHandler(ILogger<OrderStartedIntegrationEventHandler> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(_mediator));

    }

    public async Task Consume(ConsumeContext<OrderStartedIntegrationEvent> context)
    {
        var @event = context.Message;
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            await _mediator.Send(new DeleteBasketCommand {BuyerId= @event.UserId });
        }
    }
}


