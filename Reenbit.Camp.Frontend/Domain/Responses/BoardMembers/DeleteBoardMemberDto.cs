using Domain.Responses.BoardMembers;

namespace Domain.Responses.BoardMembers;

public class DeleteBoardMemberDto
{
    public int RemovedMemberId { get; set; }
    
    public BoardMemberDto? NewOwner { get; set; }
}