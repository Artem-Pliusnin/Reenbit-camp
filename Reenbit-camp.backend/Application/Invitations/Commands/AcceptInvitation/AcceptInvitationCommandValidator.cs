using FluentValidation;

namespace Application.Invitations.Commands.AcceptInvitation;

internal class AcceptInvitationCommandValidator 
    : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.InvitationId)
            .GreaterThan(0)
            .NotEmpty();
    }
}