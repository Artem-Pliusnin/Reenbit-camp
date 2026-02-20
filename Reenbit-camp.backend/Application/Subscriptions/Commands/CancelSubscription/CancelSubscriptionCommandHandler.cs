using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.CancelSubscription;

internal class CancelSubscriptionCommandHandler : ICommandHandler<CancelSubscriptionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStripeService _stripeService;

    public CancelSubscriptionCommandHandler(
        IUnitOfWork unitOfWork,
        IStripeService stripeService)
    {
        _unitOfWork = unitOfWork;
        _stripeService = stripeService;
    }
    
    public async Task<Result> Handle(
        CancelSubscriptionCommand request, 
        CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();

        var subscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (subscription is null || 
            string.IsNullOrEmpty(subscription.StripeSubscriptionId))
        {
            return Result.Failure(SubscriptionErrors.UserDataNotFound);
        }
        
        await _stripeService.CancelSubscriptionAsync(
            subscription.StripeSubscriptionId,
            cancellationToken);
            
        subscription.CancelAtPeriodEnd = true;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}