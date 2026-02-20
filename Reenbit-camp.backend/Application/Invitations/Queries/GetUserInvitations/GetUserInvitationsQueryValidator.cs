using FluentValidation;

namespace Application.Invitations.Queries.GetUserInvitations;

internal class GetUserInvitationsQueryValidator 
    : AbstractValidator<GetUserInvitationsQuery>
{
    public GetUserInvitationsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}