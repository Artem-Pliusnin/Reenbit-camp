using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Labels;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Labels.Queries.GetLabelsForCard;

public class GetLabelsForCardQueryHandler : IQueryHandler<GetLabelsForCardQuery,List<LabelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLabelsForCardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<LabelDto>>> Handle(
        GetLabelsForCardQuery request, 
        CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var card = await cardRepository
            .GetByIdWithListAsync(request.CardId, cancellationToken);
        
        if (card == null)
        {
            return Result.Failure<List<LabelDto>>(CardErrors.CardDoesNotExistError);
        }
        
        var labelRepository = _unitOfWork.GetRepository<ILabelRepository>();
        
        var labels = await labelRepository
            .GetNotСonnectedToCardAsync(request.CardId, card.List.BoardId, cancellationToken);

        var response = _mapper.Map<List<LabelDto>>(labels);
        
        return response;
    }
}