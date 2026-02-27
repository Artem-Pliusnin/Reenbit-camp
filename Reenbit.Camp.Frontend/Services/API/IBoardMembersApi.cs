using Domain.Requests.BoardMembers;
using Domain.Responses.BoardMembers;
using Refit;

namespace Services.API;

public interface IBoardMembersApi
{
    [Get("/Board/{boardId}/BoardMembers/")]
    Task<ApiResponse<List<BoardMemberDto>>> GetByBoardAsync(int boardId);
    
    [Get("/Board/{boardId}/BoardMembers/current")]
    Task<ApiResponse<BoardMemberDto>> GetCurrentAsync(int boardId);
    
    [Put("/Board/{boardId}/BoardMembers/{id}/role")]
    Task<ApiResponse<object>> UpdateRoleAsync(
        int boardId,
        int id,
        [Body] UpdateBoardMemberRoleRequest request);
    
    [Delete("/Board/{boardId}/BoardMembers/{id}")]
    Task<ApiResponse<DeleteBoardMemberDto>> DeleteAsync(int boardId, int id);
}
