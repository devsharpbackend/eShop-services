
namespace eShop.Services.CatalogAPI.Domain.AggregatesModel.CatalogAggregate;

public class CatalogItem : Entity, IAggregateRoot
{
    private string? _name;
    private string? _description;
    private decimal _price;
    private decimal _priceWithDiscount;
    private bool _isDiscount;
    private decimal _discount;
    private string? _pictureFileName;
    private int _catalogTypeId;
    private int _availableStock;
    private int _stockThreshold;
    private int _maxStockThreshold;

    public CatalogItem(string? name, string? description, decimal price, decimal priceWithDiscount, bool isDiscount, decimal discount, string? pictureFileName, int catalogTypeId, int availableStock, int stockThreshold, int maxStockThreshold)
    {
        this._name = name?? throw new CatalogDomainException("The name is empty and must be entered");
        this._description = description;
        this._price = price==0?throw new CatalogDomainException("The name is empty and must be entered.") :price;
        this._priceWithDiscount = priceWithDiscount;
        this._isDiscount = isDiscount;
        this._discount = discount;
        this._pictureFileName = pictureFileName;
        this._catalogTypeId = catalogTypeId;
        this._availableStock = availableStock;
        this._stockThreshold = stockThreshold;

        if (maxStockThreshold < this._availableStock)
        {
            throw new CatalogDomainException("maxStockThreshold not must be less than availableStock");
        }
        if (maxStockThreshold < this._stockThreshold)
        {
            throw new CatalogDomainException("maxStockThreshold not must be less than StockThreshold");
        }
        this._maxStockThreshold = maxStockThreshold;

        this._maxStockThreshold = maxStockThreshold;

    }

    public string Name => _name;

    public void UpdateName(string newName)
    {
        this._name = newName ?? throw new CatalogDomainException("The name is empty and must be entered");

    }
    public string Description=>_description; 

    public decimal Price => _price;

    public void UpdatePrice(decimal newPrice)
    {
        if(newPrice <= 0 ) throw new CatalogDomainException("The name is empty and must be entered.");

        if(newPrice!= this._price)
        {
            // raise Event for Price Changed
           AddDomainEvent(new ProductPriceChangedDomainEvent(newPrice,_price,this));

        }

        this._price = newPrice;

    }

    public decimal PriceWithDiscount  => _priceWithDiscount;

    public bool IsDiscount => _isDiscount; 

    public decimal Discount  => _discount; 

    public string PictureFileName => _pictureFileName;

    public int CatalogTypeId => _catalogTypeId;

   // public CatalogType CatalogType { get;  set; }



    // Quantity in stock
    public int AvailableStock  => _availableStock;

    // Available stock at which we should reorder
    public int StockThreshold  => _stockThreshold;


    // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
    public int MaxStockThreshold => _maxStockThreshold;


    public int AddStock(int quantity)
    {
        int original = this._availableStock;


        if ((original + quantity) > this.MaxStockThreshold)
        {

            this._availableStock += (this.MaxStockThreshold - this.AvailableStock);
        }
        else
        {
            this._availableStock += quantity;
        }

        //Returns the quantity that has been added to stock
        return this._availableStock - original;
    }

    public int RemoveStock(int quantityDesired)
    {
        if (this._availableStock == 0)
        {
            throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");
        }

        if (this._availableStock-quantityDesired < 0)
        {
            throw new CatalogDomainException($"Item units desired should be greater than zero");
        }

        int removed = Math.Min(quantityDesired, this.AvailableStock);

        this._availableStock -= removed;

        if (this._availableStock <= this._stockThreshold)
        {
            // Save Domain Event for publication at the time of storage
            AddDomainEvent(new ProductShortageHasOccurredDomainEvent(this._maxStockThreshold-this._availableStock,this));
        }

        return removed;
    }

    public void SetDiscount(decimal discount)
    {
        if (!this._isDiscount)
        {
            throw new CatalogDomainException($"This product is not discounted ");
        }

        if ((this.Price * 20 / 100) > discount)
        {
            throw new CatalogDomainException($"More than 20% of the product price cannot be discounted ");
        }

        this._discount = discount;
        this._priceWithDiscount = this._price - discount;
    }


}
