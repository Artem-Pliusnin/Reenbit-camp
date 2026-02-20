using FluentValidation;

namespace Application.Users.Queries.GetUserProfileInfo;

internal class GetUserProfileInfoQueryValidator 
    : AbstractValidator<GetUserProfileInfoQuery>
{
    public GetUserProfileInfoQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}