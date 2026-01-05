using Application.Abstractions.Messaging;
using Domain.DTOs.CardMembers;

namespace Application.CardMembers.Queries.GetCardMembers;

public sealed record GetCardMembersQuery(int CardId) : IQuery<List<CardMemberDto>>;