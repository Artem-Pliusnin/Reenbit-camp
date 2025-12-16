using Application.Abstractions.Messaging;
using Domain.DTOs.Cards;

namespace Application.Cards.Commands.CreateCard;

public sealed record CreateCardCommand(
    int ListId,
    string Title,
    int UserId) 
    : ICommand<CardDto>;