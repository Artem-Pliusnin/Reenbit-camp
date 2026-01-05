using Application.Abstractions.Messaging;
using Domain.DTOs.CardLabels;

namespace Application.CardLabels.Commands.CreateCardLabel;

public sealed record CreateCardLabelCommand(int CardId, int LabelId) : ICommand<CardLabelDto>;