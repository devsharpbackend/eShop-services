

namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public class GetUserOrderListQueryHandler : IRequestHandler<GetUserOrderListQuery, IEnumerable<OrderSummaryVM>>
{
    private readonly IOptionsSnapshot<OrderSetting> options;
   
    public GetUserOrderListQueryHandler(IOptionsSnapshot<OrderSetting> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
       
    }

    public async Task<IEnumerable<OrderSummaryVM>> Handle(GetUserOrderListQuery request, CancellationToken cancellationToken)
    {
        return await GetOrdersFromUserAsync(request.UserId);
    }



    public async Task<IEnumerable<OrderSummaryVM>> GetOrdersFromUserAsync(string userId)
    {
        using (var connection = new SqlConnection(this.options.Value.ConnectionString))
        {
            connection.Open();

            return await connection.QueryAsync<OrderSummaryVM>(@"SELECT o.[Id] as OrderId,o.[OrderDate] as [date],o.[OrderStatus] as [status], SUM(oi.units*oi.unitprice) as total
                    FROM [ordering].[Orders] o
                    LEFT JOIN[ordering].[orderitems] oi ON  o.Id = oi.orderid            
                    LEFT JOIN[ordering].[buyers] ob on o.BuyerId = ob.Id
                    WHERE ob.IdentityGuid = @userId
                    GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                    ORDER BY o.[Id]", new { userId });
        }
        

    }


}
