
using SystemBase.Application.Services.UserServices.Dtos;

namespace SystemBase.Application.Services.UserServices;

public interface IUserService
{
    Task<string> GetTokenAsync(TokenRequest request , CancellationToken cancellationToken = default);
     

}
