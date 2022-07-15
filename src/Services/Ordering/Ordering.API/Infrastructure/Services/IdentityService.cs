namespace eShop.Services.Ordering.OrderingAPI.Infrastructure.Services;


public class IdentityService : IIdentityService
{
    private IHttpContextAccessor _context;

    public IdentityService(IHttpContextAccessor context)
    {
         _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public string GetUserIdentity()
    {
        return "1000";
       // return _context.HttpContext.User.FindFirst("sub").Value;
    }

    public string GetUserName()
    {
        return string.Empty;
        return _context.HttpContext.User.Identity.Name;
    }
}
