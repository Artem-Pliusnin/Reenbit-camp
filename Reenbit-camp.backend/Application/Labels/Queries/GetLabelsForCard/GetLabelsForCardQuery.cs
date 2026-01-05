using Application.Abstractions.Messaging;
using Domain.DTOs.Labels;

namespace Application.Labels.Queries.GetLabelsForCard;

public sealed record GetLabelsForCardQuery(int CardId) : IQuery<List<LabelDto>>;