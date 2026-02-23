using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Cards.Commands.CreateCard;

public class CreateCardCommandValidator 
    : AbstractValidator<CreateCardCommand>
{
    public CreateCardCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.ListId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Title)
            .MaximumLength(CardValidationConstants.TitleMaxLength)
            .MinimumLength(CardValidationConstants.TitleMinLength)
            .NotEmpty();
    }
}