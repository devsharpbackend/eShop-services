namespace eShop.Services.Discount.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;
    public DiscountController(IDiscountRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));      
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
      
       return CreatedAtRoute("GetDiscount", new { catalogId = discount.CatalogId }, discount);
    }

}

