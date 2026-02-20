using FluentValidation;

namespace Application.Subscriptions.Commands.CancelSubscription;

internal class CancelSubscriptionCommandValidator 
    : AbstractValidator<CancelSubscriptionCommand>
{
    public CancelSubscriptionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}