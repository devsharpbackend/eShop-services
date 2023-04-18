using Microsoft.AspNetCore.Authentication;
namespace eShop.ApiGateways.Web.Bff.Shopping.Web.Shopping.HttpAggregator.Infrastructure;


public interface ITokenProvider
{
    Task<string> GetTokenAsync();
}

public class AppTokenProvider : ITokenProvider
{
    private string _token;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppTokenProvider( IHttpContextAccessor httpContextAccessor)
    {
       
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string?> GetTokenAsync()
    {
        const string ACCESS_TOKEN = "access_token";

        return await _httpContextAccessor.HttpContext
            .GetTokenAsync(ACCESS_TOKEN);
    }
   
}