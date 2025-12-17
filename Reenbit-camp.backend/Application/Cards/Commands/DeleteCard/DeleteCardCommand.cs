using Application.Abstractions.Messaging;

namespace Application.Cards.Commands.DeleteCard;

public record DeleteCardCommand(int CardId) : ICommand;