using Application.Abstractions.Messaging;

namespace Application.Labels.Commands.DeleteLabel;

public sealed record DeleteLabelCommand(int LabelId) : ICommand;