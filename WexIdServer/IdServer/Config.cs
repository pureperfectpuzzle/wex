using Duende.IdentityServer.Models;

namespace IdServer
{
	public class Config
	{
		public static IEnumerable<IdentityResource> IdentityResources =>
			new IdentityResource[]
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				/*new IdentityResource
				{
					Name = "role",
					UserClaims = new List<string> { "role" }
				}*/
			};

		public static IEnumerable<ApiScope> ApiScopes =>
			new ApiScope[]
			{
				new ApiScope("wexassessmentapi_scope"),
			};

		public static IEnumerable<ApiResource> ApiResources =>
			new ApiResource[]
			{
				new ApiResource("wexassessmentapi")
				{
					Scopes = new List<string> { "wexassessmentapi_scope" },
					ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256()) },
					UserClaims = new List<string> { "role" }
				}
			};

		public static IEnumerable<Client> Clients =>
			new Client[]
			{
                // api client
                new Client
				{
					ClientId = "api.client",
					ClientName = "API Test Client",

					AllowedGrantTypes = GrantTypes.ClientCredentials,
					ClientSecrets = { new Secret("5C35BA65-4E20-43B2-90D4-9E2EA7A56DE1".Sha256()) },

					AllowedScopes = { "wexassessmentapi_scope" }
				}
			};
	}
}
