
namespace SystemBase.Domain.Settings;

public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public string EncryptKey { get; set; }
    public TimeSpan NotBeforeMinutes { get; set; }
    public TimeSpan Expire { get; set; }
}
