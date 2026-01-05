using Application.Abstractions.Messaging;
using Domain.DTOs.Labels;

namespace Application.Labels.Commands.CreateLabel;

public sealed record CreateLabelCommand(
    int BoardId, 
    string Text, 
    string Color) 
    : ICommand<LabelDto>;