
namespace eShop.Services.Discount.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;
    private readonly IZeroMqPublisher _zeroMqPublisher;
    public DiscountController(IDiscountRepository repository, IZeroMqPublisher zeroMqPublisher)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));      
        _zeroMqPublisher = zeroMqPublisher ?? throw new ArgumentNullException(nameof(zeroMqPublisher));
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
       _zeroMqPublisher.Publish(new DiscountCreatedIntegrationEvent(discount.CatalogId, discount.Amount));
       return CreatedAtRoute("GetDiscount", new { catalogId = discount.CatalogId }, discount);
    }

}

