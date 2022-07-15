using System.Net;

namespace eShop.Services.Catalog.CatalogAPI.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly ICatalogRepository _catalogRepository;


    public CatalogController(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    [HttpGet]
    [Route("Items")]
    [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
    public async  Task<IActionResult> ItemsAsync()
    {
        var _list =await _catalogRepository.GetByPagingAsync(10, 1);
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
        var item = await _catalogRepository.GetByIdAsync(id);

        if(item == null)
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
    public async Task<ActionResult> UpdateProductNameAndPrice([FromBody] CatalogItemDto productToUpdate)
    {
        var catalogItem = await _catalogRepository.GetByIdAsync(productToUpdate.Id);

        if (catalogItem == null)
        {
            return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
        }

        catalogItem.UpdateName(productToUpdate.Name);  
        catalogItem.UpdatePrice(productToUpdate.Price);

        await _catalogRepository.UnitOfWork.SaveEntitiesAsync();

        return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
    }


    [Route("UpdateStock")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateStock([FromBody] CatalogItemStockDto productToUpdate)
    {
        var catalogItem = await _catalogRepository.GetByIdAsync(productToUpdate.Id);

        if (catalogItem == null)
        {
            return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
        }

        catalogItem.RemoveStock(productToUpdate.Quantity);

        await _catalogRepository.UnitOfWork.SaveEntitiesAsync();


        return CreatedAtAction(nameof(ItemByIdAsync), new { id = productToUpdate.Id }, null);
    }

}

public class CatalogItemDto
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public decimal Price { get; set; }
}
public class CatalogItemStockDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}