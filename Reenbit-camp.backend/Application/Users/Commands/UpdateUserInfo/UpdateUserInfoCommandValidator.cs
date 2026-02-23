using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Users.Commands.UpdateUserInfo;

internal class UpdateUserInfoCommandValidator 
    : AbstractValidator<UpdateUserInfoCommand>
{
    public UpdateUserInfoCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(UserValidationConstants.FirstNameMaxLength)
            .MinimumLength(UserValidationConstants.FirstNameMinLength)
            .NotEmpty();
        
        RuleFor(x => x.LastName)
            .MaximumLength(UserValidationConstants.LastNameMaxLength)
            .MinimumLength(UserValidationConstants.LastNameMinLength)
            .NotEmpty();
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}