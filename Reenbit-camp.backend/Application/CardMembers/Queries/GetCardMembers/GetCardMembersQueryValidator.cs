using FluentValidation;

namespace Application.CardMembers.Queries.GetCardMembers;

internal class GetCardMembersQueryValidator 
    : AbstractValidator<GetCardMembersQuery>
{
    public GetCardMembersQueryValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}