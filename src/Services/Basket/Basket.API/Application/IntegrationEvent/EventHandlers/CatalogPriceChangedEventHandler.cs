
using eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.UpdateBasketCatalogPrice;

namespace eShop.Services.Basket.BasketAPI.Application.IntegrationEvents.EventHandlers;

public class CatalogPriceChangedEventHandler : IZeroMqConsumer<CatalogPriceChangedIntegrationEvent>
{

    private readonly ILogger<CatalogPriceChangedEventHandler> _logger;

    private readonly IMediator _mediator;

    public CatalogPriceChangedEventHandler(ILogger<CatalogPriceChangedEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(_mediator));

    }

    public async Task Consume(CatalogPriceChangedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            await _mediator.Send(new UpdateBasketCatalogPriceCommand()
            {
                CatalogId = @event.CatalogID,
                NewPrice = @event.NewPrice,
                OldPrice = @event.OldPrice
            });
        }
    }
}
