

using Microsoft.EntityFrameworkCore;
using SystemBase.Application.Common.Exceptions;
using SystemBase.Application.Common.Interfaces;
using SystemBase.Application.Services.UserServices.Dtos;
using SystemBase.Domain.Interfaces;
using SystemBase.Utility.Helpers;

namespace SystemBase.Application.Services.UserServices;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _db;
    private readonly ITokenBuilderService _tokenBuilderService;
    public UserService(ITokenBuilderService tokenBuilderService,
        IApplicationDbContext db)
    {
        _db = db;
        _tokenBuilderService = tokenBuilderService;
    }

    public async Task<string> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users.SingleOrDefaultAsync(c => c.UserName == request.UserName);

        if (user == null)
            throw new BadRequestException("نام کاربری صحیح نیست");

        if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new BadRequestException("رمز عبور صحیح نیست");

        string token = await _tokenBuilderService.GetJweTokenAsync(user, cancellationToken);

        return token;
    }


}
