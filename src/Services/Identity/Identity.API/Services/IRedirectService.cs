namespace eShop.Services.IdentityAPI.Services;


public interface IRedirectService
{
    string ExtractRedirectUriFromReturnUrl(string url);
}
