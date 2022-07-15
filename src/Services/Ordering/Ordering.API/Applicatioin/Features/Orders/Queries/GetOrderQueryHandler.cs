

namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public class GetOrderQueryHandler:IRequestHandler<GetOrderQuery, OrderVM>
{
    private readonly IOptionsSnapshot<OrderSetting> options;
 
    public GetOrderQueryHandler(IOptionsSnapshot<OrderSetting> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
       
    }

    public Task<OrderVM> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return GetOrderAsync(request.OrderId);
    }

    public async Task<OrderVM> GetOrderAsync(int Id)
    {
        using (var connection = new SqlConnection(this.options.Value.ConnectionString))
        {
            connection.Open();

            var result = await connection.QueryAsync<dynamic>(
                @"select o.[Id] as OrderId,o.OrderDate as date, o.Description as description,
                    o.Address_City as city, o.Address_Country as country, o.Address_State as state, o.Address_Street as street, o.Address_ZipCode as zipcode,
                    o.OrderStatus as status, o.OrderNumber,b.Name as BuyerName, 
                    oi.ProductName as productname,oi.ProductId as productId, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                    FROM ordering.buyers b inner join ordering.Orders o on o.BuyerId=b.id
                    LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
                    WHERE o.Id=@id"
                    , new { Id }
                );

            if (result.AsList().Count == 0)
                throw new KeyNotFoundException();

            return MapOrderItems(result);
        }
    }


    private OrderVM MapOrderItems(dynamic result)
    {
        var order = new OrderVM
        {
            OrderId = result[0].OrderId,
            OrderNumber=result[0].OrderNumber,
            date = result[0].date,
            status = result[0].status,
            description = result[0].description,
            street = result[0].street,
            city = result[0].city,
            zipcode = result[0].zipcode,
            country = result[0].country,
            orderitems = new List<OrderitemVM>(),
            total = 0
        };

        foreach (dynamic item in result)
        {
            var orderitem = new OrderitemVM
            {
                productname = item.productname,
                units = item.units,
                unitprice = (double)item.unitprice,
                pictureurl = item.pictureurl,
                productId = item.productId,
            };

            order.total += item.units * item.unitprice;
            order.orderitems.Add(orderitem);
        }

        return order;
    }

 
}
