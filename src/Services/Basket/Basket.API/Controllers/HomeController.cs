
namespace eShop.Services.Basket.BasketAPI.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}
