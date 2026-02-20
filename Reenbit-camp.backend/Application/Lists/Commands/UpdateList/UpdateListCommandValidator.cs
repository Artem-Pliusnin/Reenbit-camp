using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Lists.Commands.UpdateList;

internal class UpdateListCommandValidator 
    : AbstractValidator<UpdateListCommand>
{
    public UpdateListCommandValidator()
    {
        RuleFor(x => x.ListId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Title)
            .MaximumLength(ListValidationConstants.TitleMaxLength)
            .MinimumLength(ListValidationConstants.TitleMinLength)
            .NotEmpty();
    }
}