using FluentValidation;

namespace Application.Cards.Commands.UpdateCardPosition;

internal class UpdateCardPositionCommandValidator 
    : AbstractValidator<UpdateCardPositionCommand>
{
    public UpdateCardPositionCommandValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.NewPosition)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.NewListId)
            .GreaterThan(0)
            .NotEmpty();
    }
}