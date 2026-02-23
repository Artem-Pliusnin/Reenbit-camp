using FluentValidation;

namespace Application.CardAttachments.Commands.DeleteCardAttachment;

internal class DeleteCardAttachmentCommandValidator 
    : AbstractValidator<DeleteCardAttachmentCommand>
{
    public DeleteCardAttachmentCommandValidator()
    {
        RuleFor(x => x.CardAttachmentId)
            .GreaterThan(0)
            .NotEmpty();
    }
}