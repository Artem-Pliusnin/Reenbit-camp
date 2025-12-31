using Application.Abstractions.Messaging;
using Domain.DTOs.CardLabels;

namespace Application.CardLabels.Queries.GetCardLabels;

public sealed record GetCardLabelsQuery(int CardId) : IQuery<List<CardLabelDto>>;