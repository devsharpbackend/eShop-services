namespace eShop.Services.Basket.BasketAPI.Application.Features.Basket.Queries.GetBasket;


public class GetBasketQueryHandler :IRequestHandler<GetCustomerBasketQuery, CustomerBasketVM>
{
    private readonly IBasketRepository _repository;

    private readonly ILogger<GetBasketQueryHandler> _logger;

    public GetBasketQueryHandler(
        ILogger<GetBasketQueryHandler> logger,
        IBasketRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<CustomerBasketVM?> Handle(GetCustomerBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _repository.GetBasketAsync(request.CustomerId);

        if (basket != null)
        {
            // Mapping CustomerBasket Domain To CustomerBasketVM
            CustomerBasketVM customerBasketVM = new CustomerBasketVM
            {
                BasketItems = basket.Items.Select(p => new BasketItemVM
                {
                    Id = p.Id,
                    OldUnitPrice = p.OldUnitPrice,
                    PictureUrl = p.PictureUrl,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList(),
                BuyerId = basket.BuyerId
            };

            return customerBasketVM;
        }

        return null;

    }
}
