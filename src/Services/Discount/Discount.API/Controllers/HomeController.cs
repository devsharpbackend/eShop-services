
namespace eShop.Services.Discount.DiscountAPI.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}
