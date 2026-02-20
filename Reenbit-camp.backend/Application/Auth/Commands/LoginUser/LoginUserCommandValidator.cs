using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Auth.Commands.LoginUser;

internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(UserValidationConstants.EmailMaxLength)
            .EmailAddress()
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .MaximumLength(UserValidationConstants.PasswordMaxLength)
            .MinimumLength(UserValidationConstants.PasswordMinLength)
            .NotEmpty();
    }
    
}