using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.SubscriptionDeleted;

internal class SubscriptionDeletedCommandHandler : ICommandHandler<SubscriptionDeletedCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IBoardsStatusesService _boardsStatusesService;

    public SubscriptionDeletedCommandHandler(
        IUnitOfWork unitOfWork, 
        IBoardsStatusesService boardsStatusesService)
    {
        _unitOfWork = unitOfWork;
        _boardsStatusesService = boardsStatusesService;
    }

    public async Task<Result> Handle(SubscriptionDeletedCommand request, CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
        var subscriptionPlansRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();

        var subscription = await userSubscriptionsRepository
            .GetBySubscriptionId(request.SubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return Result.Success();
        }
        
        var freePlan = await subscriptionPlansRepository
            .GetBySubscriptionName("Free", cancellationToken);

        subscription.SubscriptionPlanId = freePlan!.Id;
        subscription.SubscriptionStatus = SubscriptionStatus.Active;
        subscription.StripeSubscriptionId = null;
        subscription.CancelAtPeriodEnd = false;
        subscription.CurrentPeriodEnd = null;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _boardsStatusesService.UpdateUserBoardsStatuses(
            subscription.UserId, 
            freePlan.BoardsLimit);
        
        return Result.Success();
    }
}