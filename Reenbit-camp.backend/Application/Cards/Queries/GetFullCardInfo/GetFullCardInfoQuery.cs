using Application.Abstractions.Messaging;
using Domain.DTOs.Cards;

namespace Application.Cards.Queries.GetFullCardInfo;

public sealed record GetFullCardInfoQuery(int Id) : IQuery<CardInfoDto>;