using Domain.Models.BoardMembers;
using Domain.Requests.BoardMembers;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IBoardMembersService
{
    Task<Result<List<BoardMemberModel>>> GetByBoardAsync(int boardId);
    
    Task<Result<BoardMemberModel>> GeCurrentAsync(int boardId);

    Task<Result<object>> UpdateRoleAsync(int id, UpdateBoardMemberRoleRequest request);
}