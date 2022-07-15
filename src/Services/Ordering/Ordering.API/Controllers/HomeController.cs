
namespace eShop.Services.Ordering.OrderingAPI.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}
