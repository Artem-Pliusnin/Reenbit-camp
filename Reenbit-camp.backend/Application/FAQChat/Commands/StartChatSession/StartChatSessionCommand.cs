using Application.Abstractions.Messaging;

namespace Application.FAQChat.Commands.StartChatSession;

public sealed record StartChatSessionCommand(int UserId) : ICommand;