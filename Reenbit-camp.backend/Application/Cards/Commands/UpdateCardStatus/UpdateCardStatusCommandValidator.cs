using FluentValidation;

namespace Application.Cards.Commands.UpdateCardStatus;

internal class UpdateCardStatusCommandValidator 
    : AbstractValidator<UpdateCardStatusCommand>
{
    public UpdateCardStatusCommandValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.IsCompleted)
            .NotEmpty();
    }
}