using Domain.DTOs.CardMembers;

namespace Domain.DTOs.Cards;

public class UpdatedCardMembersDto
{
    public int CardId {get;set;}
    
    public List<CardMemberDto> Members {get;set;}
}