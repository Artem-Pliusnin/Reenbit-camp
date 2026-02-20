using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Labels.Commands.CreateLabel;

internal class CreateLabelCommandValidator 
    : AbstractValidator<CreateLabelCommand>
{
    public CreateLabelCommandValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Text)
            .MaximumLength(LabelValidationConstants.LabelNameMaxLength)
            .NotEmpty();
        
        RuleFor(x => x.Color)
            .Length(LabelValidationConstants.ColorValueLength)
            .NotEmpty();
    }
}