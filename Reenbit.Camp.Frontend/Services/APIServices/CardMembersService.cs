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
    
    public async Task<Result<List<CardMemberModel>>> GetByCardAsync(int cardId)
    {
        var response = await _cardMemnbersApi.GetByCardAsync(cardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<CardMemberModel>>(content));
    }

    public async Task<Result<CardMemberModel>> CreateAsync(CreateCardMemberRequest request)
    {
        var response = await _cardMemnbersApi.CreateAsync(request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<CardMemberModel>(content));
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _cardMemnbersApi.DeleteAsync(id);

        return response.HandleResult();
    }
}