using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.CardMembers;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardMembers.Queries.GetCardMembers;

internal class GetCardMembersQueryHandler : IQueryHandler<GetCardMembersQuery, List<CardMemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCardMembersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<CardMemberDto>>> Handle(
        GetCardMembersQuery request, 
        CancellationToken cancellationToken)
    {
        var cardMembersRepository = _unitOfWork.GetRepository<ICardMembersRepository>();
        
        var cardMembers = await cardMembersRepository
            .GetByCardIdAsync(request.CardId, cancellationToken);

        var response = _mapper.Map<List<CardMemberDto>>(cardMembers);
        
        return response;
    }
}