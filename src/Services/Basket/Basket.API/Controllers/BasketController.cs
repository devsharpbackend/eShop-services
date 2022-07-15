
namespace  eShop.Services.Basket.BasketAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator,
        ILogger<BasketController> logger,
        IBasketRepository repository )
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerBasketVM), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync(string id)
    {
        var basket =await _mediator.Send(new GetCustomerBasketQuery {CustomerId=id });

        return Ok(basket );
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasketAsync([FromBody] UpdateBasketCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBasketByIdAsync), new { id = command.BuyerId }, null);
        }
        catch(Exception ex)
        {
            return Ok();
        }
    }

    
    // DELETE api/values/5
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasketByIdAsync(string id)
    {
        await _mediator.Send(new DeleteBasketCommand {BuyerId=id });

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
