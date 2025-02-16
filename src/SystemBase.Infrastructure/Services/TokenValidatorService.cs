
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SystemBase.Domain.Interfaces;

namespace SystemBase.Infrastructure.Services;

internal interface ITokenValidatorService
{
    Task Execute(TokenValidatedContext context);
}

internal class TokenValidatorService : ITokenValidatorService
{
    private readonly IAppLogger<TokenValidatorService> _appLogger;
    public TokenValidatorService(IAppLogger<TokenValidatorService> appLogger)
    {
        _appLogger = appLogger;
    }

    public async Task Execute(TokenValidatedContext context)
    {
        var claimsIdentity = context.Principal;

        var userId = claimsIdentity.FindFirst("UserId")?.Value;

        if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
        {
            context.Fail("claims not found ...");
            return;
        }

        if (!int.TryParse(userId, out int userLong))
        {
            context.Fail("claims not found ...");
            return;
        }

        /*string path = context.Request.Path;
        if (roles.EmptyOrNull() && !path.StartsWith("/shop"))
        {
            _appLogger.LogWarning($"user {userId} hasn't role.");
            context.Fail("claims not found ...");
            return;
        }*/

        await Task.CompletedTask;

    }
}
