using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Queries.HasMinimumPlan;

internal class HasMinimumPlanQueryHandler : IQueryHandler<HasMinimumPlanQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public HasMinimumPlanQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        HasMinimumPlanQuery request, 
        CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();
        
        var subscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (subscription is null)
        {
            return Result.Failure<bool>(SubscriptionErrors.UserDataNotFound);
        }

        return subscription.SubscriptionPlanId >= (int)request.MinimumPlan;
    }
}