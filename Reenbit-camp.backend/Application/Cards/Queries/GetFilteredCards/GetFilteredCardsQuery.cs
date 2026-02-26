using Application.Abstractions.Messaging;
using Domain.DTOs.Cards;
using Domain.DTOs.Shared;
using Domain.Models;

namespace Application.Cards.Queries.GetFilteredCards;

public sealed record GetFilteredCardsQuery(
    int UserId, 
    CardsFilterModel Filter) 
    : IQuery<InfiniteScrollDto<CardDto>>;