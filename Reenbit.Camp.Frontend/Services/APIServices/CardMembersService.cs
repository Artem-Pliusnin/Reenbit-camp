using AutoMapper;
using Domain.Models.CardMembers;
using Domain.Models.Labels;
using Domain.Requests.CardMembers;
using Domain.Requests.Labels;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class CardMembersService : ICardMembersService
{
    private readonly ICardMembersApi _cardMemnbersApi;
    private readonly IMapper _mapper;

    public CardMembersService(ICardMembersApi cardMemnbersApi, IMapper mapper)
    {
        _cardMemnbersApi = cardMemnbersApi;
        _mapper = mapper; 
    }
    
    public async Task<Result<List<CardMemberModel>>> GetByCardAsync(int boardId, int cardId)
    {
        var response = await _cardMemnbersApi.GetByCardAsync(boardId, cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CardMemberModel>>(content));
    }

    public async Task<Result<CardMemberModel>> CreateAsync(int boardId, CreateCardMemberRequest request)
    {
        var response = await _cardMemnbersApi.CreateAsync(boardId, request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardMemberModel>(content));
    }

    public async Task<Result<object>> DeleteAsync(int boardId, int id)
    {
        var response = await _cardMemnbersApi.DeleteAsync(boardId, id);

        return response.HandleResult();
    }
}