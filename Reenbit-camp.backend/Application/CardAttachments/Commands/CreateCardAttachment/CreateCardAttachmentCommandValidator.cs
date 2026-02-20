using Domain.Constants.ValidationConstants;
using Domain.Entities;
using FluentValidation;

namespace Application.CardAttachments.Commands.CreateCardAttachment;

internal class CreateCardAttachmentCommandValidator 
    : AbstractValidator<CreateCardAttachmentCommand>
{
    public CreateCardAttachmentCommandValidator()
    {
        RuleFor(x => x.FileName)
            .MaximumLength(CardAttachmentValidationConstants.FileNameMaxLength)
            .MinimumLength(CardAttachmentValidationConstants.FileNameMinLength)
            .NotEmpty();
        
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}