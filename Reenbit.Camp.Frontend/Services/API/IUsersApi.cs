using Domain.Models.Boards;
using Domain.Requests.Users;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Domain.Responses.UserAvatars;
using Domain.Responses.Users;
using Refit;

namespace Services.API;

public interface IUsersApi
{
    [Get("/Users/{id}")]
    Task<ApiResponse<UserDto>> GetUserInfoAsync(
        int id);
    
    [Get("/Users/{id}/profile")]
    Task<ApiResponse<UserProfileDto>> GetUserProfileAsync(
        int id);
    
    [Put("/Users/{id}")]
    Task<ApiResponse<object>> UpdateUserInfoAsync(
        [Body] UpdateUserInfoRequest request,
        int id);
    
    [Multipart]
    [Put("/Users/{id}/avatar")]
    Task<ApiResponse<UserAvatarDto>> UpdateUserAvatarAsync(
        [AliasAs("File")] StreamPart file,
        int id);
    
    [Put("/Users/{id}/password")]
    Task<ApiResponse<object>> UpdateUserPasswordAsync(
        [Body] UpdateUserPasswordRequest request,
        int id);
    
    [Get("/Users/invitation/suggestions")]
    Task<ApiResponse<List<UserDto>>> GetInvitationSuggestionsAsync(
        [Query] GetInviteSuggestionRequest request);
    
    [Get("/Users/not-connected/card/{id}")]
    Task<ApiResponse<List<UserDto>>> GetNotConnectedToCardAsync(int id);
}