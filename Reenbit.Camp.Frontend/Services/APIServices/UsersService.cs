using AutoMapper;
using Domain.Models.Users;
using Domain.Requests.Users;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class UsersService : IUsersService
{
    private readonly IUsersApi _usersApi;
    private readonly IMapper _mapper;

    public UsersService(IUsersApi usersApi, IMapper mapper)
    {
        _usersApi = usersApi;
        _mapper = mapper;
    }
    
    public async Task<Result<List<UserModel>>> GetInvitationSuggestionsAsync(GetInviteSuggestionRequest request)
    {
        var response = await _usersApi.GetInvitationSuggestionsAsync(request);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<UserModel>>(content));
    }

    public async Task<Result<List<UserModel>>> GetNotConnectedToCardAsync(int cardId)
    {
        var response = await _usersApi.GetNotConnectedToCardAsync(cardId);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<UserModel>>(content));
    }
}