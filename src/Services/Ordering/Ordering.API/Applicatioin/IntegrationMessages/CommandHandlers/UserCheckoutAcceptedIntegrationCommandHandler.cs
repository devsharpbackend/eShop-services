using eShop.BuildingBlocks.Event.CommonEvent.Commands;
using eShop.BuildingBlocks.Event.CommonEvent.Responses;
using eShop.Services.Ordering.OrderingAPI.Application.Features.Behaviors.Idempotency;

namespace eShop.Services.CatalogAPI.Application.IntegrationMessages.CommandHandlers;
public class UserCheckoutAcceptedIntegrationCommandHandler : IConsumer<UserCheckoutAcceptedIntegrationCommand>
{

    private readonly ILogger<UserCheckoutAcceptedIntegrationCommandHandler> _logger;
    private readonly IMediator _mediator;
    public UserCheckoutAcceptedIntegrationCommandHandler(ILogger<UserCheckoutAcceptedIntegrationCommandHandler> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<UserCheckoutAcceptedIntegrationCommand> context)
    {
        try
        {
            var @event = context.Message;
            if(@event.RequestId==Guid.Empty)
            throw new OrderingApplicationException(new JsonErrorResponse { Messages = "RequestId Is Empty or not Exists"});

            using (LogContext.PushProperty("IntegrationCommandContext ", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration Command: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
               
                var createOrderCommand = new CreateOrderCommand(@event.Basket.BasketItems, @event.UserId, @event.UserName, @event.City, @event.Street,
                   @event.State, @event.Country, @event.ZipCode,
                   @event.CardNumber, @event.CardHolderName, @event.CardExpiration,
                   @event.CardSecurityNumber, @event.CardTypeId);

                var identifiedCommand = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, context.Message.RequestId);
                var result =  await _mediator.Send(identifiedCommand);

                if (result)
                {
                    _logger.LogInformation("----- CreateOrderCommand suceeded - RequestId: {RequestId}", @event.RequestId);
                }
                else
                {
                    _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", @event.RequestId);
                }

                await context.RespondAsync(new CreateOrderResponse { });
            }
        }
        catch (OrderingApplicationException ex)
        {
            _logger.LogError("Error in DiscountCreatedEventHandler ");
            await context.RespondAsync(ex.JsonErrorResponse);
        }
    }

   
}
