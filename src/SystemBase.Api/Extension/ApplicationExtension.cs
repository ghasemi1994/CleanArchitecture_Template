using Insurance.Apis.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;

namespace SystemBase.Api.Extension
{
    public static class ApplicationExtension
    {



        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .Enrich.FromLogContext()
             .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

        }
        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(Options =>
            {
                Options.SwaggerDoc("v1", new OpenApiInfo { Title = "InsuranceApi", Version = "v1" });
                var security = new OpenApiSecurityScheme
                {
                    Name = "JWT Auth",
                    Description = "enter your token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                Options.AddSecurityDefinition(security.Reference.Id, security);
                Options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                 {security,new string[]{ } }
            });
            });
        }
        public static void AddCorsPolicies(this WebApplicationBuilder builder, IConfiguration configuration)
        {
           /* var requestPolicies = configuration.GetSection("RequestPolicies:AcceptOrigins");

            var origins = requestPolicies.Get<string[]>();

            if (origins == null)
                throw new ArgumentNullException("RequestPolicies:AcceptOrigins ==> section is null in `appsettings.json` ");*/

            builder.Services.AddCors(option =>
            {
                option.AddDefaultPolicy(builder =>
                {
                    //builder.WithOrigins(origins);
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });

            });
        }

        public static void UseApplicationSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                Options =>
                {
                    Options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    Options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    Options.OAuthUsePkce();
                });
        }
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
