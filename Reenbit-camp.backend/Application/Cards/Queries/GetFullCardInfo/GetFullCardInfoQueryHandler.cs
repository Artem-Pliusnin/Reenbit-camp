using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Cards;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Queries.GetFullCardInfo;

internal class GetFullCardInfoQueryHandler  : IQueryHandler<GetFullCardInfoQuery, CardInfoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFullCardInfoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<CardInfoDto>> Handle(
        GetFullCardInfoQuery request, 
        CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var cards = await cardRepository
            .GetByIdAsync(request.Id, cancellationToken);

        var response = _mapper.Map<CardInfoDto>(cards);
        
        return response;
    }
}