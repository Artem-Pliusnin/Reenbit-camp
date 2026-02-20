using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Lists.Commands.CreateList;

internal class CreateListCommandValidator 
    : AbstractValidator<CreateListCommand>
{
    public CreateListCommandValidator()
    {
        RuleFor(x => x.BoardId)
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