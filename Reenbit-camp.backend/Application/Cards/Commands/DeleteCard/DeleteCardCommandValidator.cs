using FluentValidation;

namespace Application.Cards.Commands.DeleteCard;

internal class DeleteCardCommandValidator 
    : AbstractValidator<DeleteCardCommand>
{
    public DeleteCardCommandValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}