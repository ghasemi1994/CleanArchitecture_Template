using Microsoft.AspNetCore.Http;
using SystemBase.Domain.Interfaces;

namespace SystemBase.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
        => _httpContextAccessor = httpContextAccessor;



    public int UserId => 1;//Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimConstant.UserId));
    public string UserName => "";//_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
    public List<int?> Roles => new List<int?>(); //_httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)?.Split(',').ToList().ToListInt32();
    public string IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "";
    public HttpContext HttpContext => _httpContextAccessor.HttpContext;

}
