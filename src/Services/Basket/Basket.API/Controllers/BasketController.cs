

using eShop.BuildingBlocks.Event.CommonEvent.Responses;

namespace eShop.Services.Basket.BasketAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;
  //  private readonly IdentityService _identityService;
    private readonly ILogger<BasketController> _logger;
    private readonly IRequestClient<UserCheckoutAcceptedIntegrationCommand> _userCheckoutClient;

    public BasketController(IMediator mediator,
        ILogger<BasketController> logger, //IdentityService identityService,
        IRequestClient<UserCheckoutAcceptedIntegrationCommand> userCheckoutClient
        )
    {
       // _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userCheckoutClient = userCheckoutClient ?? throw new ArgumentNullException(nameof(userCheckoutClient));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerBasketVM), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
    {
        var basket = await _mediator.Send(new GetCustomerBasketQuery { CustomerId = id });

        return Ok(basket);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasketAsync([FromBody] UpdateBasketCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBasketByIdAsync), new { id = command.BuyerId }, null);
        }
        catch (Exception ex)
        {
            return Ok();
        }
    }


    // DELETE api/values/5
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasketByIdAsync(string id)
    {
        await _mediator.Send(new DeleteBasketCommand { BuyerId = id });

        return Ok();

    }


    [Route("checkout")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckoutCommand command, [FromHeader(Name = "x-requestid")] string requestId)
    {
        command.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
           guid : command.RequestId;

        await _mediator.Send(command);
        return Accepted();
    }

}
