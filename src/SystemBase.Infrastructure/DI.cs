using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemBase.Application.Common.Interfaces;
using SystemBase.Domain.Interfaces;
using SystemBase.Domain.Settings;
using SystemBase.Infrastructure.Extensions;
using SystemBase.Infrastructure.Logging;
using SystemBase.Infrastructure.Persistence.Context;
using SystemBase.Infrastructure.Services;


namespace SystemBase.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, option)
            => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        //config
        services.Configure<JwtSettings>(configuration.GetSection("JWTSettings"));


        //services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenBuilderService, TokenBuilderService>();
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));


        //extension
        services.AddAuthenticateWithJWE(configuration);

        return services;
    }
}
