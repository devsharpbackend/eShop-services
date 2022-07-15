namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.CreateOrder.Command;

public class CreateCatalogCommand:IRequest<int>
{
    [JsonConstructor]
    public CreateCatalogCommand(string name, string description, decimal price, bool isDiscount, string pictureFileName, int catalogTypeId, int availableStock, int stockThreshold, int maxStockThreshold)
    {
        Name = name;
        Description = description;
        Price = price;
        IsDiscount = isDiscount;
        PictureFileName = pictureFileName;
        CatalogTypeId = catalogTypeId;
        AvailableStock = availableStock;
        StockThreshold = stockThreshold;
        MaxStockThreshold = maxStockThreshold;
    }

    [JsonProperty]
    public string Name {get; set;}
    [JsonProperty]
    public string Description { get; set; }
    [JsonProperty]
    public decimal Price { get; set; }
    [JsonProperty]
    public bool IsDiscount  {get;  set; }
    [JsonProperty]
    public string PictureFileName { get;  set; }
    [JsonProperty]
    public int CatalogTypeId { get;  set; }
    [JsonProperty]
    public int AvailableStock { get;  set; }
    [JsonProperty]
    public int StockThreshold { get;  set; }
    [JsonProperty]
    public int MaxStockThreshold { get;  set; }

}
