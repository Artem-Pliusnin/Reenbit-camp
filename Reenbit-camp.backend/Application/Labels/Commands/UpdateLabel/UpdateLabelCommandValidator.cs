using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Labels.Commands.UpdateLabel;

internal class UpdateLabelCommandValidator 
    : AbstractValidator<UpdateLabelCommand>
{
    public UpdateLabelCommandValidator()
    {
        RuleFor(x => x.LabelId)
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