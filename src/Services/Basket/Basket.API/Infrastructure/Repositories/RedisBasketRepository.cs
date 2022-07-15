namespace eShop.Services.Basket.BasketAPI.Infrastructure.Repositories;

public class RedisBasketRepository : IBasketRepository
{
    private readonly ILogger<RedisBasketRepository> _logger;
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisBasketRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer redis)
    {
        _logger = loggerFactory.CreateLogger<RedisBasketRepository>();
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }

    public IEnumerable<string> GetUsers()
    {
        // read kesy from Redis
        var server = GetServer();
        var data = server.Keys();

        return data?.Select(k => k.ToString());
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId)
    {
        var data = await _database.StringGetAsync(customerId);

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        // Due to DDD limitations in serialization
        var ob = JsonSerializer.Deserialize<RedisCustomerBasket>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // map To Domain Model 
        var customerBasket = new CustomerBasket(ob.BuyerId);
        foreach (var item in ob.Items)
        {
            customerBasket.AddBasketItem(item.Id, item.Quantity, item.ProductId, item.ProductName,
                item.PictureUrl, item.UnitPrice, item.OldUnitPrice);
        }

        return customerBasket;
    }

    // these Clasess are created due to DDD limitations in serializing these classes
    class RedisCustomerBasket
    {
        public RedisCustomerBasket()
        {
            Items = new List<RedisBasketItem>();
        }
        public string BuyerId { get; set; }

        public List<RedisBasketItem> Items { get; set; }
    }
    class RedisBasketItem
    {
        
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var created = await _database.StringSetAsync(basket.BuyerId, JsonSerializer.Serialize(basket));

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the item.");
            return null;
        }

        _logger.LogInformation("Basket item persisted succesfully.");

        return await GetBasketAsync(basket.BuyerId);
    }

    private IServer GetServer()
    {
        var endpoint = _redis.GetEndPoints();
        return _redis.GetServer(endpoint.First());
    }
}
