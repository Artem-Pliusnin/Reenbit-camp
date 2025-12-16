using Application.Abstractions.Messaging;
using Domain.DTOs.Cards;

namespace Application.Cards.Queries.GetCardsByList;

public sealed record GetCardsByListQuery(int ListId) : IQuery<List<CardDto>>;