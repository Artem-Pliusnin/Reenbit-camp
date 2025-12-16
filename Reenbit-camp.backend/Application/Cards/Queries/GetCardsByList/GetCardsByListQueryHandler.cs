using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Cards;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Queries.GetCardsByList;

internal class GetCardsByListQueryHandler 
    : IQueryHandler<GetCardsByListQuery, List<CardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCardsByListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<CardDto>>> Handle(
        GetCardsByListQuery request, 
        CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var cards = await cardRepository
            .GetByListIdAsync(request.ListId, cancellationToken);

        var response = _mapper.Map<List<CardDto>>(cards);
        
        return response;
    }
}