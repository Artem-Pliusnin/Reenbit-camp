using Application.Abstractions.Messaging;
using Domain.DTOs.Labels;

namespace Application.Labels.Queries.GetBoardLabels;

public sealed record GetBoardLabelsQuery(int BoardId) : IQuery<List<LabelDto>>;