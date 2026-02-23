using FluentValidation;

namespace Application.BoardMembers.Queries.GetСurrentBoardMember;

internal class GetСurrentBoardMemberQueryValidator 
    : AbstractValidator<GetСurrentBoardMemberQuery>
{
    public GetСurrentBoardMemberQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}