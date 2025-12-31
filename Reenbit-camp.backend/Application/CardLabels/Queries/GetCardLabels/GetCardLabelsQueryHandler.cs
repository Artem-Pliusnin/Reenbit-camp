using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.CardLabels;
using Domain.DTOs.Labels;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardLabels.Queries.GetCardLabels;

internal class GetCardLabelsQueryHandler : IQueryHandler<GetCardLabelsQuery, List<CardLabelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCardLabelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<CardLabelDto>>> Handle(
        GetCardLabelsQuery request, 
        CancellationToken cancellationToken)
    {
        var cardLabelRepository = _unitOfWork.GetRepository<ICardLabelsRepository>();
        
        var cardLabels = await cardLabelRepository
            .GetByCardIdAsync(request.CardId, cancellationToken);

        var response = _mapper.Map<List<CardLabelDto>>(cardLabels);
        
        return response;
    }
}