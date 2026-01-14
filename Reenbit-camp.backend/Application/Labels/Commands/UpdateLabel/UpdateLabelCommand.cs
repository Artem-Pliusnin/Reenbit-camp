using Application.Abstractions.Messaging;
using Domain.DTOs.Labels;

namespace Application.Labels.Commands.UpdateLabel;

public sealed record UpdateLabelCommand( 
    int LabelId,
    string Text,
    string Color) : ICommand<UpdatedLabelDto>;