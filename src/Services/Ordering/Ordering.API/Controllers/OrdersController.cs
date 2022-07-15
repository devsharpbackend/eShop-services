namespace eShop.Services.Ordering.OrderingAPI.Controllers;


[Route("api/v1/[controller]")]
//[Authorize]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    //private readonly IOrderQueries _orderQueries;
    private readonly IIdentityService _identityService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IIdentityService identityService,
        IMediator mediator,ILogger<OrdersController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }

    [Route("cancel")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CancelOrderAsync([FromBody] CancelOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
    {
        bool commandResult = await _mediator.Send(command);
        if (!commandResult)
        {
            return BadRequest();
        }
        return Ok();
    }


    [Route("{orderId:int}")]
    [HttpGet]
    [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> GetOrderAsync(int orderId)
    {
       var order= await _mediator.Send(new GetOrderQuery { OrderId= orderId });
        return Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderSummaryVM>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderSummaryVM>>> GetOrdersAsync()
    {
        var userid = _identityService.GetUserIdentity();
        var orders = await _mediator.Send(new GetUserOrderListQuery() {UserId= userid });

        return Ok(orders);
    }

   

   
}
