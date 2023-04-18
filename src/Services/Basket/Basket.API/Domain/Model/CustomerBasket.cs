using Newtonsoft.Json;

namespace eShop.Services.Basket.BasketAPI.Domain.Model;

public class CustomerBasket: IAggregateRoot
{
    private readonly  string _buyerId;
    public string BuyerId => _buyerId;

 
    private readonly List<BasketItem> _items;

    public ICollection<BasketItem> Items => _items;



    public CustomerBasket(string BuyerId)
    {
        _buyerId = BuyerId;
        this._items = new List<BasketItem>();
    }

   
    public void AddBasketItem(string id, int quantity, int productId, string productName, string pictureUrl, decimal unitPrice, decimal oldUnitPrice)
    {
        _items.Add(new BasketItem(id,quantity,productId,productName,pictureUrl,unitPrice,oldUnitPrice));
    }
}

