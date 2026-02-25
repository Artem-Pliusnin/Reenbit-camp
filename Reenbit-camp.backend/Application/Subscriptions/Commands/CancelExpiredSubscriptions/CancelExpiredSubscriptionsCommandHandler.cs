using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.CancelExpiredSubscriptions;

internal class CancelExpiredSubscriptionsCommandHandler : ICommandHandler<CancelExpiredSubscriptionsCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBoardsStatusesService _boardsStatusesService;

    public CancelExpiredSubscriptionsCommandHandler(
        IUnitOfWork unitOfWork, 
        IBoardsStatusesService boardsStatusesService)
    {
        _unitOfWork = unitOfWork;
        _boardsStatusesService = boardsStatusesService;
    }

    public async Task<Result> Handle(CancelExpiredSubscriptionsCommand request, CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
        var subscriptionPlansRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();
        
        var userSubscriptions = await userSubscriptionsRepository
            .GetExpiredSubscriptionsAsync(DateTime.UtcNow, cancellationToken);

        var freePlan = await subscriptionPlansRepository
            .GetBySubscriptionName("Free", cancellationToken);
        
        foreach (var subscription in userSubscriptions)
        {
            subscription.SubscriptionPlanId = freePlan!.Id;
            subscription.SubscriptionStatus = SubscriptionStatus.Active;
            subscription.StripeSubscriptionId = null;
            subscription.CancelAtPeriodEnd = false;
            subscription.CurrentPeriodEnd = null;
            
            userSubscriptionsRepository.Update(subscription);
            await _boardsStatusesService.UpdateUserBoardsStatuses(
                subscription.UserId, 
                freePlan.BoardsLimit);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}