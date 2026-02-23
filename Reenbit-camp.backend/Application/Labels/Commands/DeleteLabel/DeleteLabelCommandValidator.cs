using FluentValidation;

namespace Application.Labels.Commands.DeleteLabel;

internal class DeleteLabelCommandValidator 
    : AbstractValidator<DeleteLabelCommand>
{
    public DeleteLabelCommandValidator()
    {
        RuleFor(x => x.LabelId)
            .GreaterThan(0)
            .NotEmpty();
    }
}