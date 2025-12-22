namespace Domain.Requests.Cards;

public sealed record UpdateCardRequest(
    int CardId, 
    string Title,
    string? Description);