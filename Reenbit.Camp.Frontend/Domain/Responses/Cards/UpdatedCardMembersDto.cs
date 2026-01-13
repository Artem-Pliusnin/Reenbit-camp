using Domain.Responses.CardMembers;

namespace Domain.Responses.Cards;

public record UpdatedCardMembersDto(
    int CardId,
    List<CardMemberDto> Members);