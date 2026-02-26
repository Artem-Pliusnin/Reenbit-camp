using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Cards;
using Domain.DTOs.Shared;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Queries.GetFilteredCards;

internal class GetFilteredCardsQueryHandler 
    : IQueryHandler<GetFilteredCardsQuery, InfiniteScrollDto<CardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IMapper _mapper;

    public GetFilteredCardsQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<InfiniteScrollDto<CardDto>>> Handle(
        GetFilteredCardsQuery request, 
        CancellationToken cancellationToken)
    {
        var cardsRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var infiniteScrollDto = await cardsRepository
            .GetFilteredAsync(request.UserId, request.Filter, cancellationToken);

        var cards = _mapper.Map<List<CardDto>>(infiniteScrollDto.Dtos);
        
        return new InfiniteScrollDto<CardDto>()
        {
            Dtos = cards,
            HasMore = infiniteScrollDto.HasMore
        };
    }
}