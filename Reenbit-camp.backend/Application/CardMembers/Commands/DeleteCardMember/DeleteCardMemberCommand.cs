using Application.Abstractions.Messaging;

namespace Application.CardMembers.Commands.DeleteCardMember;

public sealed record DeleteCardMemberCommand(int CardMemberId) : ICommand;