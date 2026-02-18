using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.CheckoutSessionCompleted;

internal class CheckoutSessionCompletedCommandHandler : ICommandHandler<CheckoutSessionCompletedCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStripeService _stripeService;

    public CheckoutSessionCompletedCommandHandler(
        IUnitOfWork unitOfWork, 
        IStripeService stripeService)
    {
        _unitOfWork = unitOfWork;
        _stripeService = stripeService;
    }
    
    public async Task<Result> Handle(CheckoutSessionCompletedCommand request, CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
        var subscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (subscription is null)
        {
            return Result.Failure(SubscriptionErrors.UserDataNotFound);
        }
        
        string? oldSubscriptionId = String.Empty;
        
        if (!string.IsNullOrEmpty(subscription.StripeSubscriptionId) &&
            subscription.StripeSubscriptionId != request.SubscriptionId &&
            subscription.SubscriptionStatus == SubscriptionStatus.Active)
        {
            oldSubscriptionId = subscription.StripeSubscriptionId;
        }
        
        subscription.SubscriptionPlanId = request.PlanId;
        subscription.StripeSubscriptionId = request.SubscriptionId;
        subscription.SubscriptionStatus = SubscriptionStatus.Active;
        subscription.CancelAtPeriodEnd = false;
        subscription.CurrentPeriodEnd = request.SubscriptionPeriodEnd;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(oldSubscriptionId))
        {
            try
            {
                await _stripeService.CancelSubscriptionImmediatelyAsync(
                    oldSubscriptionId, 
                    cancellationToken);
            }
            catch (Exception e)
            {
                return Result.Success();
            }
        }
        
        return Result.Success();
    }
}