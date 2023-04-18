namespace eShop.Services.Basket.BasketAPI.Infrastructure.Services;

public class BasketIdentityService : IBasketIdentityService
{
    private IHttpContextAccessor _context;

    public BasketIdentityService(IHttpContextAccessor context)
    {
         _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string GetUserIdentity()
    {
        //return "1000";
        return _context.HttpContext.User.FindFirst("sub").Value;
    }
}

