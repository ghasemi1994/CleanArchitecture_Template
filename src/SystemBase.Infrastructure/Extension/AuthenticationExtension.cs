
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SystemBase.Infrastructure.Services;

namespace SystemBase.Infrastructure.Extensions
{
    public static class AuthenticationExtension
    {
        /// <summary>
        /// jwe simple 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticateWithJWE(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(Options =>
            {
                Options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(configureOptions =>
           {
               configureOptions.TokenValidationParameters = new TokenValidationParameters()
               {
                   // The signing key must match!
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),

                   TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:EncryptKey"])),

                   // Validate the JWT Issuer (iss) claim
                   ValidateIssuer = false,
                   ValidIssuer = configuration["JwtSettings:Issuer"],

                   // Validate the JWT Audience (aud) claim
                   ValidateAudience = false,
                   ValidAudience = configuration["JwtSettings:Audience"],

                   // Validate the token expiry
                   ValidateLifetime = true, //here we are saying that we don't care about the token's expiration date
                   RequireExpirationTime = false,

                   RequireSignedTokens = true,
                   ClockSkew = TimeSpan.Zero,

               };
               configureOptions.SaveToken = true;
               configureOptions.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           //context.Response.Headers.Add("Token-Expired", "true");
                           //context.Response.Headers.Add("access-control-expose-headers", "Token-Expired");
                       }

                       return Task.CompletedTask;
                   },
                   OnTokenValidated = context =>
                   {
                       var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                       return tokenValidatorService.Execute(context);

                   },
                   OnChallenge = context =>
                   {
                       context.HandleResponse();

                       context.Response.StatusCode = 401;

                       context.Response.ContentType = "application/json";

                       return Task.CompletedTask;

                   },
                   OnMessageReceived = context =>
                   {
                       return Task.CompletedTask;

                   },
                   OnForbidden = context =>
                   {
                       context.Response.StatusCode = 403;
                       context.Response.ContentType = "application/json";
                       return Task.CompletedTask;

                   }
               };

           });


            services.AddScoped<ITokenValidatorService, TokenValidatorService>();

            return services;
        }

      
    }
}
