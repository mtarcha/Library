using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

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

        public static IEnumerable<ApiResource> GetApis =>
            new[]
            {
                new ApiResource("MyApi1"),
                new ApiResource("MyApi2"),
            };

        public static IEnumerable<Client> GetClients =>
            new[]
            {
                new Client
                {
                    ClientId = "my_client1_id",
                    ClientSecrets = { new Secret("my_client1_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        "MyApi1", "MyApi2",
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        JwtClaimTypes.Role,
                    },
                    RedirectUris = { "http://localhost:7777/signin-oidc" },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireConsent = false
                }
            };
    }
}