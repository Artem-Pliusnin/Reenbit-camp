using Domain.Models.UserAvatars;
using Domain.Models.Users;
using Domain.Requests.Cards;
using Domain.Requests.Users;
using Domain.Responses.UserAvatars;
using Domain.Shared;
using Refit;

namespace Services.Abstractions.Services;

public interface IUsersService
{
    Task<Result<UserModel>> GetUserInfoAsync(int userId);
    
    Task<Result<UserProfileModel>> GetUserProfileAsync(int userId);

    Task<Result<object>> UpdateUserInfoAsync(UpdateUserInfoRequest request, int userId);
    
    Task<Result<UserAvatarModel>> UpdateUserAvatarAsync(StreamPart file, int userId);
    
    Task<Result<List<UserModel>>> GetInvitationSuggestionsAsync(GetInviteSuggestionRequest request);
    
    Task<Result<List<UserModel>>> GetNotConnectedToCardAsync(int cardId);
}