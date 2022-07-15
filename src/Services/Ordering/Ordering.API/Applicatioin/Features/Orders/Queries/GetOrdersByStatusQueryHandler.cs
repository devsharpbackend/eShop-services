namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public class GetOrdersByStatusQueryHandler : IRequestHandler<GetOrdersByStatusQuery, IEnumerable<OrderVM>>
{

    private readonly IOptionsSnapshot<OrderSetting> options;

    public GetOrdersByStatusQueryHandler(IOptionsSnapshot<OrderSetting> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IEnumerable<OrderVM>> Handle(GetOrdersByStatusQuery request, CancellationToken cancellationToken)
    {
        return await GetSubmittedOrders(request.orderStatus);
    }

    private async Task<IEnumerable<OrderVM>> GetSubmittedOrders(OrderStatus status)
    {
        IEnumerable<OrderVM> orderIds = new List<OrderVM>();

        int OrderStatus = (int)status;
        using (var conn = new SqlConnection(options.Value.ConnectionString))
        {
                conn.Open();
                orderIds = await conn.QueryAsync<OrderVM>(
                    @"select o.[Id] as OrderId,o.OrderDate as date, o.Description as description,
                    o.Address_City as city, o.Address_Country as country, o.Address_State as state, o.Address_Street as street, o.Address_ZipCode as zipcode,
                    o.OrderStatus as status, o.OrderNumber,
                    oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                    FROM ordering.Orders o
                    LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
                    WHERE  o.OrderStatus = @OrderStatus", new { OrderStatus });
        }

        return orderIds;
    }

}