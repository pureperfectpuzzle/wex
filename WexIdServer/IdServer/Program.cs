using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace IdServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add Duende ID Server as a service
			builder.Services.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseSuccessEvents = true;

				options.EmitStaticAudienceClaim = true;
			})
			.AddTestUsers(TestUsers.Users)
			.AddInMemoryClients(Config.Clients)
			.AddInMemoryApiResources(Config.ApiResources)
			.AddInMemoryApiScopes(Config.ApiScopes)
			.AddInMemoryIdentityResources(Config.IdentityResources);

			var app = builder.Build();

			// Add ID Server into pipeline
			app.UseIdentityServer();

			app.MapGet("/", () => "Wex Assessment Id Server");

			app.Run();
		}
	}
}
