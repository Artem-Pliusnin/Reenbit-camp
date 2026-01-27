using Application.Abstractions.Messaging;

namespace Application.FAQChat.Commands.EndChatSession;

public sealed record EndChatSessionCommand(int UserId) : ICommand;