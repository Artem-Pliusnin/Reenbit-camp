using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Commands.SubscriptionUpdated;

internal class SubscriptionUpdatedCommandHandler : ICommandHandler<SubscriptionUpdatedCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionUpdatedCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(SubscriptionUpdatedCommand request, CancellationToken cancellationToken)
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

        return Result.Success();
    }
}