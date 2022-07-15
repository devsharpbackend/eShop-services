
using static eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Commands.CreateOrderCommand;

namespace eShop.Services.Ordering.Application.IntegrationMessages.CommandHandlers;
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
            using (LogContext.PushProperty("IntegrationCommandContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);


                List<OrderItemDTO> OrderItems = new List<OrderItemDTO>();
                foreach (var item in @event.Basket.BasketItems)
                {
                    OrderItems.Add(new OrderItemDTO
                    {
                        Discount = 0,
                        PictureUrl = item.PictureUrl,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Units = item.Quantity
                    });
                }

                var createOrderCommand = new CreateOrderCommand(OrderItems, @event.UserId, @event.UserName, @event.City, @event.Street,
                   @event.State, @event.Country, @event.ZipCode,
                   @event.CardNumber, @event.CardHolderName, @event.CardExpiration,
                   @event.CardSecurityNumber, @event.CardTypeId);

                await _mediator.Send(createOrderCommand);

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
