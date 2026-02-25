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
    
    private readonly IBoardsStatusesService _boardsStatusesService;

    public CheckoutSessionCompletedCommandHandler(
        IUnitOfWork unitOfWork, 
        IBoardsStatusesService boardsStatusesService)
    {
        _unitOfWork = unitOfWork;
        _boardsStatusesService = boardsStatusesService;
    }
    
    public async Task<Result> Handle(CheckoutSessionCompletedCommand request, CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
        var subscriptionsPlanRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();
        
        var subscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (subscription is null)
        {
            return Result.Failure(SubscriptionErrors.UserDataNotFound);
        }
        
        var newSubscription = await subscriptionsPlanRepository
            .GetByIdAsync(
                request.PlanId, 
                cancellationToken);
        
        subscription.SubscriptionPlanId = request.PlanId;
        subscription.StripeSubscriptionId = request.SubscriptionId;
        subscription.SubscriptionStatus = SubscriptionStatus.Active;
        subscription.CancelAtPeriodEnd = false;
        subscription.CurrentPeriodEnd = request.SubscriptionPeriodEnd;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _boardsStatusesService
            .UpdateUserBoardsStatuses(
                request.UserId, 
                newSubscription.BoardsLimit);
        
        return Result.Success();
    }
}