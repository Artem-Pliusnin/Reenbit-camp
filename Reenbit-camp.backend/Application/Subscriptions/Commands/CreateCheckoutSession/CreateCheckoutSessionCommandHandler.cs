using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.CreateCheckoutSession;

internal class CreateCheckoutSessionCommandHandler 
    : ICommandHandler<CreateCheckoutSessionCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStripeService _stripeService;

    public CreateCheckoutSessionCommandHandler(
        IUnitOfWork unitOfWork,
        IStripeService stripeService)
    {
        _unitOfWork = unitOfWork;
        _stripeService = stripeService;
    }
    
    public async Task<Result<string>> Handle(
        CreateCheckoutSessionCommand request, 
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        var subscriptionPlansRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<string>(UserErrors.UserDoesNotExistError);
        }

        var plan = await subscriptionPlansRepository.GetByIdAsync(
            request.SubscriptionPlanId, 
            cancellationToken);
            
        if (plan is null)
        {
            return Result.Failure<string>(SubscriptionErrors.PlanNotFound);
        }

        var currentSubscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (currentSubscription is null)
        {
            return Result.Failure<string>(SubscriptionErrors.UserDataNotFound);
        }
        
        var customerId = currentSubscription.StripeCustomerId;
        if (string.IsNullOrEmpty(customerId))
        {
            customerId = await _stripeService.CreateCustomerAsync(
                user.Email,
                $"{user.FirstName} {user.LastName}",
                cancellationToken);
            
            currentSubscription.StripeCustomerId = customerId;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            customerId,
            plan.StripePriceId,
            request.SuccessUrl,
            request.CancelUrl,
            new Dictionary<string, string>
            {
                { "user_id", request.UserId.ToString() },
                { "plan_id", request.SubscriptionPlanId.ToString() }
            },
            cancellationToken);

        return sessionUrl;
    }
}