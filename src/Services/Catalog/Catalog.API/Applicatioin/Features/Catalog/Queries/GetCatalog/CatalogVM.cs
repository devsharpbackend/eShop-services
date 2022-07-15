namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalog;

public class CatalogVM
{
    public int ID { get; set; }
    public string Name {get; set;}

    public string Description  {get; set;}

    public decimal Price  {get; set;}

    public decimal PriceWithDiscount { get; set; }

    public bool IsDiscount { get; set; }

    public decimal Discount { get; set; }

    public string PictureFileName { get; set; }

    public int CatalogTypeId { get; set; }

    public int AvailableStock { get; set; }

    public int StockThreshold { get; set; }
  
    public int MaxStockThreshold { get; set; }
}
