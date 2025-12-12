using Application.Abstractions.Messaging;

namespace Application.Lists.Commands.DeleteList;

public record DeleteListCommand(int ListId) : ICommand;