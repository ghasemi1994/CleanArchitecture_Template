using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemBase.Application.Services.UserServices;
using SystemBase.Application.Services.UserServices.Dtos;

namespace SystemBase.Api.Controllers;

public class AccountController : BaseControllerApi
{

    private readonly IUserService _userService;
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPost("[action]") , AllowAnonymous]
    public async Task<IActionResult> GetToken(TokenRequest request, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.GetTokenAsync(request , cancellationToken));
    }
}
