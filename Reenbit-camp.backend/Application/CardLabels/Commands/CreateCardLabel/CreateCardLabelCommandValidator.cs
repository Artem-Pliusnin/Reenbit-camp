using FluentValidation;

namespace Application.CardLabels.Commands.CreateCardLabel;

internal class CreateCardLabelCommandValidator 
    : AbstractValidator<CreateCardLabelCommand>
{
    public CreateCardLabelCommandValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.LabelId)
            .GreaterThan(0)
            .NotEmpty();
    }
}