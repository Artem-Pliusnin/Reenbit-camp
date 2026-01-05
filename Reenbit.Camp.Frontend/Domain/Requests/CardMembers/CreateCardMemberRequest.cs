namespace Domain.Requests.CardMembers;

public sealed record CreateCardMemberRequest(int CardId, int UserId);