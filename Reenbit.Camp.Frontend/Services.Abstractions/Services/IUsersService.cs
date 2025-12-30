using Domain.Models.Users;
using Domain.Requests.Users;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IUsersService
{
    Task<Result<List<UserModel>>> GetInvitationSuggestionsAsync(GetInviteSuggestionRequest request);
}