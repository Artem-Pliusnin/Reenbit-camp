using FluentValidation;

namespace Application.Subscriptions.Queries.GetUserSubscription;

internal class GetUserSubscriptionQueryValidator 
    : AbstractValidator<GetUserSubscriptionQuery>
{
    public GetUserSubscriptionQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}