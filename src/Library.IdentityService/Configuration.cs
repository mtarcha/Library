using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;
using Library.IdentityService.Models;
using Microsoft.Extensions.Configuration;

namespace Library.IdentityService
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(), 
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = JwtClaimTypes.Role,
                    DisplayName = JwtClaimTypes.Role,
                    UserClaims = { JwtClaimTypes.Role }
                }
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var mvcClientConfig = new LibraryMvcClientConfiguration();
            configuration.GetSection(nameof(LibraryMvcClientConfiguration)).Bind(mvcClientConfig);

            return new[]
            {
                new Client
                {
                    ClientId = mvcClientConfig.ClientName,
                    ClientSecrets = { new Secret(mvcClientConfig.ClientSecret.ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        JwtClaimTypes.Role,
                    },
                    RedirectUris = { $"{mvcClientConfig.RedirectBaseUrl}signin-oidc" },
                    PostLogoutRedirectUris = { $"{mvcClientConfig.RedirectBaseUrl}Books/Search" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false
                }
            };
        }
    }
}