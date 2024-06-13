using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using WexAssessmentApi.Data;
using WexAssessmentApi.Interfaces;

namespace WexAssessmentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add a singleton instance into service container which used by Controller
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Allow user doing authentication in Swagger UI
            builder.Services.AddSwaggerGen(options =>
            {
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter token",
					Name = "Authentication",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
                });

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
	            {
		            {
			            new OpenApiSecurityScheme
			            {
				            Reference = new OpenApiReference
				            {
					            Type=ReferenceType.SecurityScheme,
					            Id="Bearer"
				            }
			            },
			            new string[]{}
		            }
	            });
			});

            // In real life, we need to put these information in application config file.
            // But here I simplify it for demo purpose.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:7220"; // Id server's url
                    options.Audience = "wexassessmentapi";
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use authentication service in pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
