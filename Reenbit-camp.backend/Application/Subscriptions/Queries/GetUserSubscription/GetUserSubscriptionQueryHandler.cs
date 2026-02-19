using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Subscriptions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Queries.GetUserSubscription;

internal class GetUserSubscriptionQueryHandler : IQueryHandler<GetUserSubscriptionQuery, UserSubscriptionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserSubscriptionQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserSubscriptionDto>> Handle(
        GetUserSubscriptionQuery request, 
        CancellationToken cancellationToken)
    {
        var userSubscriptionsRepository = _unitOfWork
            .GetRepository<IUserSubscriptionsRepository>();
        
        var userSubscription = await userSubscriptionsRepository
            .GetUserSubscription(request.UserId, cancellationToken);

        if (userSubscription is null)
        {
            return Result.Failure<UserSubscriptionDto>(SubscriptionErrors.UserDataNotFound);
        }
        
        var userSubscriptionDto = _mapper.Map<UserSubscriptionDto>(userSubscription);

        return userSubscriptionDto;
    }
}