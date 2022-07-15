using Grpc.Core;
namespace GrpcBasket;

public class BasketService : Basket.BasketBase
{
  
    private readonly ILogger<BasketService> _logger;
    private readonly IMediator _mediator;
    public BasketService(IMediator mediator, ILogger<BasketService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

  
    public override async Task<CustomerBasketResponse> GetBasketById(BasketRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Begin grpc call from method {Method} for basket id {Id}", context.Method, request.Id);

        var basket = await _mediator.Send(new GetCustomerBasketQuery { CustomerId = request.Id });

        if (basket != null)
        {
            context.Status = new Status(StatusCode.OK, $"Basket with id {request.Id} do exist");

            return MapToCustomerBasketResponse(basket);
        }
        else
        {
            context.Status = new Status(StatusCode.NotFound, $"Basket with id {request.Id} do not exist");
        }

        return new CustomerBasketResponse();
    }

    public override async Task<CustomerBasketResponse> UpdateBasket(CustomerBasketRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Begin grpc call BasketService.UpdateBasketAsync for buyer id {Buyerid}", request.Buyerid);

        await _mediator.Send(MapToCustomerBasketUpdateCommand(request));

        context.Status = new Status(StatusCode.OK, $"Basket with buyer id {request.Buyerid} do not exist");

        return null;
    }

    private CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasketVM customerBasket)
    {
        var response = new CustomerBasketResponse
        {
            Buyerid = customerBasket.BuyerId
        };

        customerBasket.BasketItems.ForEach(item => response.Items.Add(new BasketItemResponse
        {
            Id = item.Id,
            Oldunitprice = (double)item.OldUnitPrice,
            Pictureurl = item.PictureUrl,
            Productid = item.ProductId,
            Productname = item.ProductName,
            Quantity = item.Quantity,
            Unitprice = (double)item.UnitPrice
        }));

        return response;
    }

    private UpdateBasketCommand MapToCustomerBasketUpdateCommand(CustomerBasketRequest customerBasketRequest)
    {
        var response = new UpdateBasketCommand
        {
            BuyerId = customerBasketRequest.Buyerid,
            BasketItems=new List<BasketItemVM>()
        };

        customerBasketRequest.Items.ToList().ForEach(item => response.BasketItems.Add(new BasketItemVM
        {
            Id = item.Id,
            OldUnitPrice = (decimal)item.Oldunitprice,
            PictureUrl = item.Pictureurl,
            ProductId = item.Productid,
            ProductName = item.Productname,
            Quantity = item.Quantity,
            UnitPrice = (decimal)item.Unitprice
        }));

        return response;
    }
}
