using AutoMapper;
using Domain.Models.UserAvatars;
using Domain.Models.Users;
using Domain.Requests.Users;
using Domain.Shared;
using Refit;
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

    public async Task<Result<UserModel>> GetUserInfoAsync(int userId)
    {
        var response = await _usersApi.GetUserInfoAsync(userId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<UserModel>(content));
    }

    public async Task<Result<UserProfileModel>> GetUserProfileAsync(int userId)
    {
        var response = await _usersApi.GetUserProfileAsync(userId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<UserProfileModel>(content));
    }

    public async Task<Result<object>> UpdateUserInfoAsync(UpdateUserInfoRequest request, int userId)
    {
        var response = await _usersApi.UpdateUserInfoAsync(request, userId);

        return response.HandleResult();
    }

    public async Task<Result<UserAvatarModel>> UpdateUserAvatarAsync(StreamPart file, int userId)
    {
        var response = await _usersApi.UpdateUserAvatarAsync(file, userId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<UserAvatarModel>(content));
    }

    public async Task<Result<object>> UpdateUserPasswordAsync(UpdateUserPasswordRequest request, int userId)
    {
        var response = await _usersApi.UpdateUserPasswordAsync(request, userId);

        return response.HandleResult();
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