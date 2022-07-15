namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Command.DeleteBasket;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
{
    private readonly IBasketRepository _repository;

    private readonly ILogger<DeleteBasketCommandHandler> _logger;

    public DeleteBasketCommandHandler(
        ILogger<DeleteBasketCommandHandler> logger,
        IBasketRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteBasketAsync(request.BuyerId);

        return Unit.Value;
    }
}
