namespace Presentation.API.Contracts.CardMembers;

public sealed record CreateCardMemberRequest(int CardId, int UserId);