namespace eShop.Services.Discount.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;
    private readonly IZeroMqPublisher _zeroMqPublisher;
    private readonly IPublishEndpoint _publishEndpoint;

    private readonly IRequestClient<DiscountCreatedIntegrationCommand> _discountCreatedClient;
    
    public DiscountController(
        IDiscountRepository repository, 
        IPublishEndpoint publishEndpoint /*IZeroMqPublisher zeroMqPublisher*/
       , IRequestClient<DiscountCreatedIntegrationCommand> discountCreatedClient
        )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));      
        //_zeroMqPublisher = zeroMqPublisher ?? throw new ArgumentNullException(nameof(zeroMqPublisher));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _discountCreatedClient=discountCreatedClient ?? throw new ArgumentNullException(nameof(discountCreatedClient));
    }

    [HttpGet("{catalogId}", Name = "GetDiscount")]
    [ProducesResponseType(typeof(CatalogDiscount), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CatalogDiscount>> GetDiscount(int catalogId)
    {
        var discount = await _repository.GetDiscount(catalogId);
        return Ok(discount);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CatalogDiscount), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CatalogDiscount>> CreateDiscount([FromBody] CatalogDiscount discount)
    {
         await _repository.CreateDiscount(discount);
        // _zeroMqPublisher.Publish(new DiscountCreatedIntegrationEvent(discount.CatalogId, discount.Amount));
        //   await _publishEndpoint.Publish(new DiscountCreatedIntegrationEvent(discount.CatalogId, discount.Amount));

        var response = await _discountCreatedClient.GetResponse<CreateDiscountValidResponse, JsonErrorResponse>(
            new DiscountCreatedIntegrationCommand(discount.CatalogId, discount.Amount));

        if (response.Is(out Response<JsonErrorResponse> responseA))
        {
            // Remove Discount

            var objectResult = new ObjectResult(responseA.Message);
            objectResult.StatusCode = responseA.Message.StatusCode;
            return objectResult;
        };



        return CreatedAtRoute("GetDiscount", new { catalogId = discount.CatalogId }, discount);
    }

}

