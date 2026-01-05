using Domain.Requests.CardMembers;
using Domain.Responses.CardMembers;
using Domain.Shared;
using Refit;

namespace Services.API;

public interface ICardMembersApi
{
    [Get("/CardMembers/card/{id}")]
    Task<ApiResponse<List<CardMemberDto>>> GetByCardAsync(int id);

    [Post("/CardMembers")]
    Task<ApiResponse<CardMemberDto>> CreateAsync(
        [Body] CreateCardMemberRequest request);
    
    [Delete("/CardMembers/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}
