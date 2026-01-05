using Application.Abstractions.Messaging;

namespace Application.Labels.Commands.UpdateLabel;

public sealed record UpdateLabelCommand( 
    int LabelId,
    string Text,
    string Color) : ICommand;