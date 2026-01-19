namespace Domain.DTOs.BoardMembers;

public class DeleteBoardMemberDto
{
    public int RemovedMemberId { get; set; }
    
    public BoardMemberDto? NewOwner { get; set; }
}