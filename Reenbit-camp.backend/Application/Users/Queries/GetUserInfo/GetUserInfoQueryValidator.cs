using FluentValidation;

namespace Application.Users.Queries.GetUserInfo;

internal class GetUserInfoQueryValidator 
    : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}