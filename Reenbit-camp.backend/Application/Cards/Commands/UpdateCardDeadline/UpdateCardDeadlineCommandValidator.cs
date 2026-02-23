using FluentValidation;

namespace Application.Cards.Commands.UpdateCardDeadline;

internal class UpdateCardDeadlineCommandValidator 
    : AbstractValidator<UpdateCardDeadlineCommand>
{
    public UpdateCardDeadlineCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}