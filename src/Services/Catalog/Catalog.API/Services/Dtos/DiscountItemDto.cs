namespace eShop.Services.Catalog.CatalogAPI.Services.Dtos;

public class DiscountItemDto
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}
