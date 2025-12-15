using Application.Abstractions.Messaging;

namespace Application.Lists.Commands.DeleteList;

public sealed record DeleteListCommand(int ListId) : ICommand;