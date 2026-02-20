using FluentValidation;

namespace Application.Users.Queries.GetInviteSuggestionUsers;

internal class GetInviteSuggestionUsersQueryValidator 
    : AbstractValidator<GetInviteSuggestionUsersQuery>
{
    public GetInviteSuggestionUsersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .NotEmpty();
    }
}