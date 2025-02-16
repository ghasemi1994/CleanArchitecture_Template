using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SystemBase.Domain.Entities;
using SystemBase.Domain.Constants;
using SystemBase.Domain.Interfaces;
using SystemBase.Domain.Settings;

namespace SystemBase.Infrastructure.Services;

public class TokenBuilderService : ITokenBuilderService
{

    private readonly JwtSettings jwtSettings;
    public TokenBuilderService(IOptions<JwtSettings> options)
    {
        jwtSettings = options.Value ?? throw new NullReferenceException("JwtSettings is null");
    }
    public async Task<string> GetJweTokenAsync(User user, CancellationToken cancellationToken)
    {
        return await _generateJweToken(user, cancellationToken);
    }

    private async Task<string> _generateJweToken(User user, CancellationToken cancellationToken)
    {

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW,
            SecurityAlgorithms.Aes128CbcHmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow.Add(jwtSettings.NotBeforeMinutes),
            Expires = DateTime.UtcNow.Add(jwtSettings.Expire),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(await _getClaims(user, cancellationToken))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
        var result = tokenHandler.WriteToken(securityToken);

        return result;
    }
    private async Task<Claim[]> _getClaims(User user, CancellationToken cancellationToken)
    {
        List<int> roles = new List<int>();

        /*if (user.UserRoles != null && user.UserRoles.Count > 0)
            roles = user.UserRoles.Select(x => x.RoleId).ToList();*/

        var claims = new[]
        {
           new Claim(ClaimConstant.UserId, user.Id.ToString()),

           new Claim(ClaimTypes.NameIdentifier, user.UserName ?? throw new NullReferenceException()),

           new Claim(ClaimTypes.Role, string.Join(',',roles)),
        };

        return await Task.FromResult<Claim[]>(claims);
    }
}
