using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Cards.Commands.UpdateCard;

internal class UpdateCardCommandValidator 
    : AbstractValidator<UpdateCardCommand>
{
    public UpdateCardCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Title)
            .MaximumLength(CardValidationConstants.TitleMaxLength)
            .MinimumLength(CardValidationConstants.TitleMinLength)
            .NotEmpty();
    }
}