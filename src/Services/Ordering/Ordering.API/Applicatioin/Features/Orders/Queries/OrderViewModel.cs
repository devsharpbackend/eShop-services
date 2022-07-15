namespace eShop.Services.Ordering.OrderingAPI.Application.Features.Orders.Queries;

public record OrderitemVM
{
    public string productname { get; init; }
    public int units { get; init; }
    public double unitprice { get; init; }
    public string pictureurl { get; init; }
}

public record OrderVM
{
    public int OrderId { get; init; }
    public DateTime date { get; init; }
    public string status { get; init; }
    public string description { get; init; }
    public string street { get; init; }
    public string city { get; init; }
    public string zipcode { get; init; }
    public string country { get; init; }
    public List<OrderitemVM> orderitems { get; set; }
    public decimal total { get; set; }
    public Guid OrderNumber { get; set; }
}

public record OrderSummaryVM
{
   
    public int OrderId { get; init; }
    public DateTime date { get; init; }
    public OrderStatus status { get; init; }
    public double total { get; init; }
}

public record CardTypeVM
{
    public int Id { get; init; }
    public string Name { get; init; }
}
