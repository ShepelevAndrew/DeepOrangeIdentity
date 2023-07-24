using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace DeepOrangeIdentity;
public class Configuration
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>() {
            new ApiScope("DeepOrangeApi", "DeepOrangeApi")
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>() {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>() {
            new ApiResource("DeepOrangeApi", "DeepOrangeApi")
            {
                Scopes = { "DeepOrangeApi" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>() {
            new Client
            {
                ClientId = "deep-orange-id",
                ClientSecrets = { new Secret("deep_secret".ToSha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = false,
                RedirectUris =
                {
                    "http://localhost:5000/auth"
                },
                PostLogoutRedirectUris =
                {
                    "tg://resolve?domain=DeepOrange_bot"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:5000"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "DeepOrangeApi"
                }
                /*AllowAccessTokensViaBrowser = true*/
            },
            new Client
            {
                ClientId = "tg-web-app",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = false,
                RedirectUris =
                {
                    "http://localhost:5013/callback.html"
                },
                PostLogoutRedirectUris =
                {
                    "http://127.0.0.1:5013/logout.html"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:5013"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "DeepOrangeApi"
                }
            }
        };
}