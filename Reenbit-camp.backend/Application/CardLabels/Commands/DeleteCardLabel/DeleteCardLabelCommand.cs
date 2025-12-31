using Application.Abstractions.Messaging;

namespace Application.CardLabels.Commands.DeleteCardLabel;

public sealed record DeleteCardLabelCommand(int CardLabelId) : ICommand;