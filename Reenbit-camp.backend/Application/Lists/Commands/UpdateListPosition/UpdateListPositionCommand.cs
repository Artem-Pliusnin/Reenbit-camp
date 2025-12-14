using Application.Abstractions.Messaging;

namespace Application.Lists.Commands.UpdateListPosition;

public sealed record UpdateListPositionCommand(
    int ListId, 
    int NewPosition,
    int UserId)
    : ICommand;