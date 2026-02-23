using FluentValidation;

namespace Application.BoardMembers.Queries.GetBoardMembers;

internal class GetBoardMembersQueryValidator 
    : AbstractValidator<GetBoardMembersQuery>
{
    public GetBoardMembersQueryValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}