
namespace eShop.Services.CatalogAPI.Application.IntegrationMessages.CommandHandlers;

public class UpdateStockForOrderIntegrationCommanddHandler :
    IConsumer<UpdateStockForOrderIntegrationCommand>
{

    private readonly ILogger<CheckStockForOrderIntegrationCommand> _logger;
    private readonly IMediator _mediator;
    public UpdateStockForOrderIntegrationCommanddHandler(
        IMediator mediator,
        ILogger<CheckStockForOrderIntegrationCommand> logger)
    {
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<UpdateStockForOrderIntegrationCommand> context)
    {
        var @commandMessage = context.Message;
        try
        {
            using (LogContext.PushProperty("IntegrationCommandContext", $"{@commandMessage.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration commnad: {IntegrationCommandId} at {AppName} - ({@IntegrationCommand})", @commandMessage.Id, Program.AppName, @commandMessage);

             

                foreach (var orderStockItem in @commandMessage.OrderStockItems)
                {
                    UpdateStockCommand updateStockCommand = new UpdateStockCommand
                    {
                        Id=orderStockItem.CatalogId,
                        Quantity= orderStockItem.Units
                    };

                   await _mediator.Send(updateStockCommand);

                }


                await context.RespondAsync(new SuccessStockUpdateResponse(@commandMessage.OrderId, @commandMessage.OrderNumber));

                
            }
        }
        catch (CatalogApplicationException ex)
        {
            _logger.LogError("Error in UpdateStockForOrderIntegrationCommanddHandler ");

            await context.RespondAsync(ex.JsonErrorResponse);
        }
    }
}
