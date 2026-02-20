using FluentValidation;

namespace Application.Invitations.Commands.DeleteInvitation;

internal class DeleteInvitationCommandValidator 
    : AbstractValidator<DeleteInvitationCommand>
{
    public DeleteInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationId)
            .GreaterThan(0)
            .NotEmpty();
    }
}