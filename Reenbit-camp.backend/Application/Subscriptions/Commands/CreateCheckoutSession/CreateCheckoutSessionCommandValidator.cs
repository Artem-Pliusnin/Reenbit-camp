using FluentValidation;

namespace Application.Subscriptions.Commands.CreateCheckoutSession;

internal class CreateCheckoutSessionCommandValidator 
    : AbstractValidator<CreateCheckoutSessionCommand>
{
    public CreateCheckoutSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.SubscriptionPlanId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.SuccessUrl)
            .NotEmpty();
        
        RuleFor(x => x.CancelUrl)
            .NotEmpty();
    }
}