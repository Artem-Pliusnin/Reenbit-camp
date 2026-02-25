using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.ResumeSubscription;

public class ResumeSubscriptionCommandHandler : ICommandHandler<ResumeSubscriptionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStripeService _stripeService;

    public ResumeSubscriptionCommandHandler(
        IUnitOfWork unitOfWork,
        IStripeService stripeService)
    {
        _unitOfWork = unitOfWork;
        _stripeService = stripeService;
    }

    public async Task<Result> Handle(ResumeSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();

        var subscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (subscription is null || 
            string.IsNullOrEmpty(subscription.StripeSubscriptionId))
        {
            return Result.Failure(SubscriptionErrors.UserDataNotFound);
        }

        if (!subscription.CancelAtPeriodEnd)
        {
            return Result.Success();
        }
        
        await _stripeService.ResumeSubscriptionAsync(
            subscription.StripeSubscriptionId,
            cancellationToken);
            
        subscription.CancelAtPeriodEnd = true;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}