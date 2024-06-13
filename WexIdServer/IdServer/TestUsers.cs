using Duende.IdentityServer.Test;
using Duende.IdentityServer;
using IdentityModel;
using static Duende.IdentityServer.Models.IdentityResources;
using System.Security.Claims;
using System.Text.Json;

namespace IdServer
{
	public class TestUsers
	{
		public static List<TestUser> Users
		{
			get
			{
				var address = new
				{
					street_address = "111 IdS Way",
					locality = "Ohio",
					postal_code = 12345,
					country = "USA"
				};

				return new List<TestUser>
				{
					new TestUser
					{
						SubjectId = "818727",
						Username = "manager",
						Password = "password",
						Claims =
						{
							new Claim(JwtClaimTypes.Name, "Manager Team"),
							new Claim(JwtClaimTypes.GivenName, "Manager"),
							new Claim(JwtClaimTypes.FamilyName, "Team"),
							new Claim(JwtClaimTypes.Email, "ManagerTeam@email.com"),
							new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
							new Claim(JwtClaimTypes.WebSite, "http://wex.com"),
							new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
						}
					}
				};
			}
		}
	}
}
