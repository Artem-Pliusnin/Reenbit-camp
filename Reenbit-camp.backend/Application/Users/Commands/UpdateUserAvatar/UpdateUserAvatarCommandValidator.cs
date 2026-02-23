using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Users.Commands.UpdateUserAvatar;

internal class UpdateUserAvatarCommandValidator 
    : AbstractValidator<UpdateUserAvatarCommand>
{
    public UpdateUserAvatarCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.FileName)
            .MaximumLength(UserAvatarsValidationConstants.FileNameMaxLength)
            .MinimumLength(UserAvatarsValidationConstants.FileNameMinLength)
            .NotEmpty();
    }
}