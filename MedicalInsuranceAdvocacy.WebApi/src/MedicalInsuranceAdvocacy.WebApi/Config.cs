using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace MedicalInsuranceAdvocacy.WebApi
{
    public class Config
    {
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("apiv1", "MedicalAdvocacy API")
            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
               new Client
                {
                    ClientId = "mdav",
                    ClientName = "MedicalAdvocacy Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                
                    RequireConsent = false,
                
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                
                    RedirectUris           = { "http://localhost:5000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5000" },
                
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "apiv1"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}
