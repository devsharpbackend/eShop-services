namespace eShop.Services.Catalog.CatalogAPI.Services;

public class DiscountService : IDiscountService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DiscountService> _logger;

    private readonly string baseAddress;

    public DiscountService(HttpClient httpClient, ILogger<DiscountService> logger,IOptions<CatalogSetting> options)
    {
        _httpClient = httpClient;
        _logger = logger;

        baseAddress = options.Value.DiscountUrl;
    }


    public async Task<DiscountItemDto?> GetDiscounts(int catalogId)
    {
        var streamTask = _httpClient.GetStreamAsync($"{baseAddress}{catalogId}");
        var repositories = await JsonSerializer.DeserializeAsync<DiscountItemDto>(await streamTask);

        return repositories;

    }

}
