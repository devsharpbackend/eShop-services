
namespace eShop.Services.CatalogAPI.Application.IntegrationMessages.CommandHandlers;

public class CheckStockForOrderIntegrationCommandHandler :
    IConsumer<CheckStockForOrderIntegrationCommand>
{

    private readonly ILogger<CheckStockForOrderIntegrationCommand> _logger;
    private readonly IMediator _mediator;
    public CheckStockForOrderIntegrationCommandHandler(
        IMediator mediator,
        ILogger<CheckStockForOrderIntegrationCommand> logger)
    {
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
    }

    public async Task Consume(ConsumeContext<CheckStockForOrderIntegrationCommand> context)
    {
        var @commandMessage = context.Message;
        try
        {
            using (LogContext.PushProperty("IntegrationCommandContext", $"{@commandMessage.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration commnad: {IntegrationCommandId} at {AppName} - ({@IntegrationCommand})", @commandMessage.Id, Program.AppName, @commandMessage);


                var confirmedOrderStockItems = new List<CheckStockResponseStockItem>();

                foreach (var orderStockItem in @commandMessage.OrderStockItems)
                {
                    var catalogItem = await _mediator.Send(new GetCatalogQuery { CatalogId = orderStockItem.CatalogId });
                    if (catalogItem != null)
                    {
                        var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                        var confirmedOrderStockItem = new CheckStockResponseStockItem(catalogItem.ID, hasStock);
                        confirmedOrderStockItems.Add(confirmedOrderStockItem);
                    }
                }

                await context.RespondAsync(new CheckStockResponse (@commandMessage.OrderId, @commandMessage.OrderNumber, confirmedOrderStockItems));

                
            }
        }
        catch (CatalogApplicationException ex)
        {
            _logger.LogError("Error in CheckStockForOrderIntegrationCommand ");

            await context.RespondAsync(ex.JsonErrorResponse);
        }
    }
}
