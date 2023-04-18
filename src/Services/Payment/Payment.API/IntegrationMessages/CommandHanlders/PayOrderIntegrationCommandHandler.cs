namespace eShop.Services.Payment.Application.IntegrationMessages.CommandHandlers;

public class PayOrderIntegrationCommandHandler :
    IConsumer<PayOrderIntegrationCommand>
{
    private readonly ILogger<PayOrderIntegrationCommandHandler> _logger;
    public PayOrderIntegrationCommandHandler(

        ILogger<PayOrderIntegrationCommandHandler> logger
        )
    {
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<PayOrderIntegrationCommand> context)
    {
        var @commandMessage = context.Message;
        try
        {
            using (LogContext.PushProperty("IntegrationCommandContext", $"{@commandMessage.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration commnad: {IntegrationCommandId} at {AppName} - ({@IntegrationCommand})", @commandMessage.Id, Program.AppName, @commandMessage);

                // The payment can be successful or it can fail
                // if payment success
                await context.RespondAsync(new CheckPaidResponse(@commandMessage.OrderId, @commandMessage.OrderNumber,false,"1411a00110","ok"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in PayOrderIntegrationCommandHandler ");

            await context.RespondAsync(new JsonErrorResponse()
            {
                StatusCode = 500,
                Messages = ex.Message
            });
        }
    }
}
