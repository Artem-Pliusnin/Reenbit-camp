using FluentValidation;

namespace Application.Invitations.Queries.GetInvitationsByBoard;

internal class GetInvitationsByBoardQueryValidator 
    : AbstractValidator<GetInvitationsByBoardQuery>
{
    public GetInvitationsByBoardQueryValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}