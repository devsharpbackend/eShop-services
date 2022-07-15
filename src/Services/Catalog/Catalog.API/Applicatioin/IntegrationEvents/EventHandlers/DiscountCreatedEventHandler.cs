namespace eShop.Services.CatalogAPI.Application.IntegrationEvents.EventHandlers;

public class DiscountCreatedEventHandler : IZeroMqConsumer<DiscountCreatedIntegrationEvent>
{

    private readonly ILogger<DiscountCreatedEventHandler> _logger;
    private readonly IMediator _mediator;
    public DiscountCreatedEventHandler(ILogger<DiscountCreatedEventHandler> logger, IMediator mediator)
    {

        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(DiscountCreatedIntegrationEvent message)
    {

        try
        {
            await _mediator.Send(new UpdateDiscountCommand
            {
                Amount = message.Amount,
                Id = message.CatalogID
            });
        }
        catch (Exception ex)
        {

            _logger.LogError("Error in DiscountCreatedEventHandler ");
        }
      
        // set Discount Catalog
        //  return Task.CompletedTask;
    }

}
