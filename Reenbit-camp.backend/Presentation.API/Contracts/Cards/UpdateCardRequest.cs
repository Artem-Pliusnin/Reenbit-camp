namespace Presentation.API.Contracts.Cards;

public sealed record UpdateCardRequest(
    int CardId, 
    string Title,
    string? Description);