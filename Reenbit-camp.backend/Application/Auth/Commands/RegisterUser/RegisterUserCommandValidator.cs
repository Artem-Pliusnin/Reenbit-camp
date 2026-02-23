using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Auth.Commands.RegisterUser;

internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(UserValidationConstants.FirstNameMaxLength)
            .MinimumLength(UserValidationConstants.FirstNameMinLength)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .MaximumLength(UserValidationConstants.LastNameMaxLength)
            .MinimumLength(UserValidationConstants.LastNameMinLength)
            .NotEmpty();
        
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