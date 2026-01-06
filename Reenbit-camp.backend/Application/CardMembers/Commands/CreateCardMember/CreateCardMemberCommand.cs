using Application.Abstractions.Messaging;
using Domain.DTOs.CardMembers;

namespace Application.CardMembers.Commands.CreateCardMember;

public sealed record CreateCardMemberCommand(int CardId, int UserId) : ICommand<CardMemberDto>;