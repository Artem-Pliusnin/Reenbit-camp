using FluentValidation;

namespace Application.CardLabels.Commands.DeleteCardLabel;

internal class DeleteCardLabelCommandValidator 
    : AbstractValidator<DeleteCardLabelCommand>
{
    public DeleteCardLabelCommandValidator()
    {
        RuleFor(x => x.CardLabelId)
            .GreaterThan(0)
            .NotEmpty();
    }
}