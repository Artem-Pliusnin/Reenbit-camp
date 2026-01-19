using Domain.Requests.BoardMembers;
using Domain.Responses.BoardMembers;
using Refit;

namespace Services.API;

public interface IBoardMembersApi
{
    [Get("/BoardMembers/board/{id}")]
    Task<ApiResponse<List<BoardMemberDto>>> GetByBoardAsync(int id);
    
    [Get("/BoardMembers/board/{id}/current")]
    Task<ApiResponse<BoardMemberDto>> GetCurrentAsync(int id);
    
    [Put("/BoardMembers/{id}/role")]
    Task<ApiResponse<object>> UpdateRoleAsync(
        int id,
        [Body] UpdateBoardMemberRoleRequest request);
    
    [Delete("/BoardMembers/{id}")]
    Task<ApiResponse<DeleteBoardMemberDto>> DeleteAsync(int id);
}
