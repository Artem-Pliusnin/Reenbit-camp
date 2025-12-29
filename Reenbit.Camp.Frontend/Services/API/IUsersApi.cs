using Domain.Models.Boards;
using Domain.Requests.Users;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Domain.Responses.Users;
using Refit;

namespace Services.API;

public interface IUsersApi
{
    [Get("/Users/invitation/suggestions")]
    Task<ApiResponse<List<UserDto>>> GetInvitationSuggestionsAsync(
        [Query] GetInviteSuggestionRequest request);
}