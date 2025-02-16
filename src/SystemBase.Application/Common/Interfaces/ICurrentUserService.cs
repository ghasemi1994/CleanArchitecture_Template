

using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace SystemBase.Domain.Interfaces;

public interface ICurrentUserService
{
    public int UserId { get; }
    public string UserName { get; }
    public List<int?> Roles { get; }
    public string IpAddress { get; }
    public HttpContext HttpContext { get; }
}
