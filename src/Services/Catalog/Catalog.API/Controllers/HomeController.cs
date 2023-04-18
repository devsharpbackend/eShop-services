
namespace eShop.Services.Catalog.CatalogAPI.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}
