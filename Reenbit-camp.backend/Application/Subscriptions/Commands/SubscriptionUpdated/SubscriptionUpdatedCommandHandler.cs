using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.SubscriptionUpdated;

internal class SubscriptionUpdatedCommandHandler : ICommandHandler<SubscriptionUpdatedCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IBoardsStatusesService _boardsStatusesService;

    public SubscriptionUpdatedCommandHandler(
        IUnitOfWork unitOfWork, 
        IBoardsStatusesService boardsStatusesService)
    {
        _unitOfWork = unitOfWork;
        _boardsStatusesService = boardsStatusesService;
    }
    
    public async Task<Result> Handle(
        SubscriptionUpdatedCommand request, 
        CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
        var subscription = await userSubscriptionsRepository
            .GetBySubscriptionId(request.SubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return Result.Success();
        }
        
        subscription.SubscriptionStatus = request.SubscriptionStatus;
        subscription.CurrentPeriodEnd = request.SubscriptionPeriodEnd;
        subscription.CancelAtPeriodEnd = request.CancelAtPeriodEnd;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _boardsStatusesService.UpdateUserBoardsStatuses(
            subscription.UserId, 
            subscription.SubscriptionPlan.BoardsLimit);

        return Result.Success();
    }
}