using Application.Abstractions.Messaging;
using Domain.DTOs.Users;

namespace Application.CardMembers.Queries.GetNotConnectedToCard;

public sealed record GetNotConnectedToCardQuery(int CardId) : IQuery<List<UserDto>>;