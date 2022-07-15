


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
            var @commnad = context.Message;

            if (@commnad.RequestId == Guid.Empty)
                throw new OrderingApplicationException(new JsonErrorResponse { Messages = "RequestId Is Empty or not Exists" });

            using (LogContext.PushProperty("IntegrationCommandContext", $"{@commnad.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationCommnadId} at {AppName} - ({@IntegrationEvent})", @commnad.Id, Program.AppName, @commnad);


                List<OrderItemDTO> OrderItems = new List<OrderItemDTO>();
                foreach (var item in @commnad.Basket.BasketItems)
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

                var createOrderCommand = new CreateOrderCommand(OrderItems, @commnad.UserId, @commnad.UserName, @commnad.City, @commnad.Street,
                   @commnad.State, @commnad.Country, @commnad.ZipCode,
                   @commnad.CardNumber, @commnad.CardHolderName, @commnad.CardExpiration,
                   @commnad.CardSecurityNumber, @commnad.CardTypeId);


                var identifiedCommand = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, context.Message.RequestId);
                var result= await _mediator.Send(identifiedCommand);

                if (result)
                {
                    _logger.LogInformation("----- CreateOrderCommand suceeded - RequestId: {RequestId}", @commnad.RequestId);
                }
                else
                {
                    _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", @commnad.RequestId);
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
