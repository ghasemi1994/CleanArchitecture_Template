
using FluentValidation;

namespace SystemBase.Application.Services.UserServices.Dtos;

public class TokenRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }

}

public class LoginRequestValidator : AbstractValidator<TokenRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(c => c.UserName).NotEmpty().NotNull();
        RuleFor(c => c.Password).NotEmpty().NotNull();
    }
}
