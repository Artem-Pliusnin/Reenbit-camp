using FluentValidation;

namespace Application.Invitations.Commands.DeclinedInvitation;

internal class DeclineInvitationCommandValidator 
    : AbstractValidator<DeclineInvitationCommand>
{
    public DeclineInvitationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.InvitationId)
            .GreaterThan(0)
            .NotEmpty();
    }
}