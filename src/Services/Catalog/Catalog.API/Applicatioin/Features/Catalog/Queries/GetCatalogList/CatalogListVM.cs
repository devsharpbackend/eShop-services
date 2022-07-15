namespace eShop.Services.Catalog.CatalogAPI.Applicatioin.Features.Catalog.Queries.GetCatalogList;


public class CatalogListVM
{
    public long TotalCount { get; set; }

    public IEnumerable<CatalogListItemVM> Data { get; set; }
}

public class CatalogListItemVM
{
    public int Id { get; set; }
    public string CatalogTypeName { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public decimal Discount { get; set; }

    public bool IsDiscount { get;set; }
    public string Name { get; set; }

    public int AvailableStock { get; set; }
}

