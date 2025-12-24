namespace Presentation.API.Contracts.Cards;

public record UpdateCardStatusRequest(
    int CardId, 
    bool IsCompleted);