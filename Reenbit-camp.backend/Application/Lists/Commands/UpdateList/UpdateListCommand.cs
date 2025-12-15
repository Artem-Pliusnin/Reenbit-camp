using Application.Abstractions.Messaging;

namespace Application.Lists.Commands.UpdateList;

public sealed record UpdateListCommand(
    int UserId,
    int ListId,
    string Title) : ICommand
{}