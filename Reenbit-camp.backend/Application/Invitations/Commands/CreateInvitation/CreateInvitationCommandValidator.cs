using FluentValidation;

namespace Application.Invitations.Commands.CreateInvitation;

internal class CreateInvitationCommandValidator 
    : AbstractValidator<CreateInvitationCommand>
{
    public CreateInvitationCommandValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.InvitedUserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.InvitedByUserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}