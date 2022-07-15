namespace eShop.Services.Basket.BasketAPI.Domain.Model;

public class BasketItem
{
    private string _id;
    private int _quantity;
    private int _productId;
    private string _productName;
    private string _pictureUrl;
    private decimal _unitPrice;
    private decimal _oldUnitPrice;

    public BasketItem(string id, int quantity, int productId, string productName, string pictureUrl, decimal unitPrice, decimal oldUnitPrice)
    {
        _id = id;
        _quantity = quantity<1?throw new BasketDomainException("Invalid number of units Quantity") :quantity;
        _productId = productId;
        _productName = productName;
        _pictureUrl = pictureUrl;
        _unitPrice = unitPrice;
        _oldUnitPrice = oldUnitPrice;
    }
    public string Id => _id;
    public int ProductId => _productId;
    public string ProductName => _productName;
    public decimal UnitPrice => _unitPrice;
    public decimal OldUnitPrice => _oldUnitPrice;
    public int Quantity => _quantity;
    public string PictureUrl => _pictureUrl;

    public void SetNewPrice(decimal newPrice)
    {
        this._oldUnitPrice = this._unitPrice;
        this._unitPrice=newPrice;
    }
}
