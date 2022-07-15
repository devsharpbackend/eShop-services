
namespace eShop.Services.CatalogAPI.Application.IntegrationMessages.EventHandlers;


public class DiscountCreatedEventHandler : IConsumer<DiscountCreatedIntegrationEvent>
{

    private readonly ILogger<DiscountCreatedEventHandler> _logger;
    private readonly IMediator _mediator;
    public DiscountCreatedEventHandler(ILogger<DiscountCreatedEventHandler> logger, IMediator mediator)
    {

        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DiscountCreatedIntegrationEvent> context)
    {

        try
        {
            var @event = context.Message;
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {

                var command = new UpdateDiscountCommand
                {
                    Amount = @event.Amount,
                    Id = @event.CatalogID
                };

                _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}:  ({@Command})",
                command.GetGenericTypeName(),
                nameof(command.Id),

                command);

                await _mediator.Send(command);
            }

        }
        catch (Exception ex)
        {

            _logger.LogError("Error in DiscountCreatedEventHandler ");
        }

        // set Discount Catalog
        //  return Task.CompletedTask;
    }

  
}


/// <summary>
/// ZeroMQ
/// </summary>
//public class DiscountCreatedEventHandler : IZeroMqConsumer<DiscountCreatedIntegrationEvent>
//{

//    private readonly ILogger<DiscountCreatedEventHandler> _logger;
//    private readonly IMediator _mediator;
//    public DiscountCreatedEventHandler(ILogger<DiscountCreatedEventHandler> logger, IMediator mediator)
//    {

//        _logger = logger;
//        _mediator = mediator;
//    }

//    public async Task Consume(DiscountCreatedIntegrationEvent @event)
//    {

//        try
//        {
          
//            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
//            {

//                var command = new UpdateDiscountCommand
//                {
//                    Amount = @event.Amount,
//                    Id = @event.CatalogID
//                };

//                _logger.LogInformation(
//                "----- Sending command: {CommandName} - {IdProperty}:  ({@Command})",
//                command.GetGenericTypeName(),
//                nameof(command.Id),

//                command);

//                await _mediator.Send(command);
//            }

//        }
//        catch (Exception ex)
//        {

//            _logger.LogError("Error in DiscountCreatedEventHandler ");
//        }
      
//        // set Discount Catalog
//        //  return Task.CompletedTask;
//    }

//}
