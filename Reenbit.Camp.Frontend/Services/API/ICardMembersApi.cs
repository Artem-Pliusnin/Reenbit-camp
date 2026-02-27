using Domain.Requests.CardMembers;
using Domain.Responses.CardMembers;
using Domain.Shared;
using Refit;

namespace Services.API;

public interface ICardMembersApi
{
    [Get("/Board/{boardId}/CardMembers/card/{id}")]
    Task<ApiResponse<List<CardMemberDto>>> GetByCardAsync(int boardId, int id);

    [Post("/Board/{boardId}/CardMembers")]
    Task<ApiResponse<CardMemberDto>> CreateAsync(
        int boardId,
        [Body] CreateCardMemberRequest request);
    
    [Delete("/Board/{boardId}/CardMembers/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}
