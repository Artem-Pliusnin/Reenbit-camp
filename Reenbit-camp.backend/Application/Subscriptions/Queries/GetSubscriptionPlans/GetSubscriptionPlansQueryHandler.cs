using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Subscriptions;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Subscriptions.Queries.GetSubscriptionPlans;

public class GetSubscriptionPlansQueryHandler 
    : IQueryHandler<GetSubscriptionPlansQuery, List<SubscriptionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSubscriptionPlansQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<SubscriptionPlanDto>>> Handle(
        GetSubscriptionPlansQuery request, 
        CancellationToken cancellationToken)
    {
        var subscriptionPlansRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();
        
        var plans = await subscriptionPlansRepository
            .GetAllPlansAsync(cancellationToken);

        var response = _mapper.Map<List<SubscriptionPlanDto>>(plans);
        
        return response;
    }
}