using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Queries.CanCreateBoard;

internal class CanCreateBoardQueryHandler : IQueryHandler<CanCreateBoardQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CanCreateBoardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        CanCreateBoardQuery request, 
        CancellationToken cancellationToken)
    {
        var userSubscriptionRepository = _unitOfWork
            .GetRepository<IUserSubscriptionsRepository>();
        var boardsRepository = _unitOfWork
            .GetRepository<IBoardRepository>();
        
        var userSubscription = await userSubscriptionRepository
            .GetUserSubscription(
                request.UserId, 
                cancellationToken);

        if (userSubscription is null)
        {
            return Result.Failure<bool>(SubscriptionErrors.UserDataNotFound);
        }

        var userBoardsCount = await boardsRepository
            .CountActiveUserOwnedBoardsAsync(request.UserId, cancellationToken);
        
        return userSubscription.SubscriptionPlan.BoardsLimit > userBoardsCount;
    }
}