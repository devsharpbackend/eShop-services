using System.Net;

namespace eShop.Services.Catalog.CatalogAPI.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
  
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
     
        this._mediator = mediator??throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [Route("Items")]
    [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CatalogListVM), (int)HttpStatusCode.OK)]
    public async  Task<IActionResult> ItemsAsync([FromQuery] int PageIndex,[FromQuery] int PageCount)
    {
        var _list = await _mediator.Send(
            new GetCatalogListQuery { PageCount = PageCount ,PageIndex= PageIndex });
        return Ok(_list);
    }
    

    [HttpGet]
    [Route("items/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CatalogItem>> ItemByIdAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }
        var item = await _mediator.Send(new GetCatalogQuery { CatalogId=id});

        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }


    //PUT api/v1/[controller]/items
    [Route("UpdateProductNameAndPrice")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateProductNameAndPrice([FromBody] UpdatePriceCommand command)
    {
        await _mediator.Send(command);
        return CreatedAtAction(nameof(ItemByIdAsync), new { id = command.Id }, null);
    }


    //POST api/v1/[controller]/items
    [Route("items")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreateProductAsync([FromBody] CreateCatalogCommand command)
    {

       int id= await _mediator.Send(command);

        return CreatedAtAction(nameof(ItemByIdAsync), new { id = id }, null);
    }

    [Route("UpdateStock")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateStock([FromBody] UpdateStockCommand command)
    {


        int removed = await _mediator.Send(command);
        return CreatedAtAction(nameof(ItemByIdAsync), new { id = command.Id }, null);
    }

}

