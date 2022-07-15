using ApiResource = IdentityServer4.Models.ApiResource;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;
using Secret = IdentityServer4.Models.Secret;

namespace eShop.Services.IdentityAPI.Configuration;

public class Config
{
    // ApiResources define the apis in your system
    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>
        {
            new ApiResource("orders", "Orders Service"),
            new ApiResource("basket", "Basket Service"),
        };
    }

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public static IEnumerable<IdentityResource> GetResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),

        };
    }

    // client want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
    {
        return new List<Client>
        {
            new Client
              {
                  ClientId = "MobileClient",
                  AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AccessTokenLifetime=60,
                  AllowOfflineAccess=true,
               AbsoluteRefreshTokenLifetime=60,
                  AllowedScopes = { "openid","profile", "basket", "orders" }

              },
             new Client
              {
                  ClientId = "Client1",
                  AllowedGrantTypes = GrantTypes.ClientCredentials,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AccessTokenLifetime=60,

                  AllowedScopes = { "openid","profile", "basket", "orders" }
              },

            new Client
              {
                  ClientId = "postmancode",
                  AllowedGrantTypes = GrantTypes.Code,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AccessTokenLifetime=60,
                   AllowOfflineAccess=true,
                    AbsoluteRefreshTokenLifetime=60,
                   AllowAccessTokensViaBrowser = true,
                   RedirectUris = { "https://oauth.pstmn.io/v1/callback"},
                   PostLogoutRedirectUris = {  "https://oauth.pstmn.io/v1/callback" },
                  AllowedScopes = { "openid","profile", "basket", "orders" }
              },
             new Client
              {
                  ClientId = "postmanImplicit",
                  AllowedGrantTypes = GrantTypes.Implicit,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AccessTokenLifetime=60,
                  AllowOfflineAccess=true,
                  AbsoluteRefreshTokenLifetime=60,
                  AllowAccessTokensViaBrowser = true,
                  RedirectUris = { "https://oauth.pstmn.io/v1/callback"},
                  PostLogoutRedirectUris = {  "https://oauth.pstmn.io/v1/callback" },
                  AllowedScopes = { "openid", "profile", "basket", "orders" }
              },
              new Client
              {
                  ClientId = "postmanHybrid",
                  AllowedGrantTypes = GrantTypes.Hybrid,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AccessTokenLifetime=60,
                  AllowOfflineAccess=true,
                  AbsoluteRefreshTokenLifetime=60,
                  AllowAccessTokensViaBrowser = true,
                  RedirectUris = { "https://oauth.pstmn.io/v1/callback"},
                  PostLogoutRedirectUris = {  "https://oauth.pstmn.io/v1/callback" },
                   AllowedScopes = { "openid","profile", "basket", "orders" }
              },
            new Client
            {
                ClientId = "basketswaggerui",
                ClientName = "Basket Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{clientsUrl["BasketApi"]}/swagger/oauth2-redirect.html",$"{clientsUrl["webshoppingapigw"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{clientsUrl["BasketApi"]}/swagger/" },

                AllowedScopes =
                {
                     IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "basket"
                }
            },
            new Client
            {
                ClientId = "orderingswaggerui",
                ClientName = "Ordering Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{clientsUrl["OrderingApi"]}/swagger/oauth2-redirect.html",$"{clientsUrl["webshoppingapigw"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{clientsUrl["OrderingApi"]}/swagger/" },

                AllowedScopes =
                {
                    "orders"
                }
            },
            new Client
            {
                ClientId = "webshoppingaggswaggerui",
                ClientName = "Web Shopping Aggregattor Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{clientsUrl["WebShoppingAgg"]}/swagger/oauth2-redirect.html",$"{clientsUrl["webshoppingapigw"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{clientsUrl["WebShoppingAgg"]}/swagger/" },

                AllowedScopes =
                {
                    "webshoppingagg",
                    "basket"
                }
            },
        };
    }
}
