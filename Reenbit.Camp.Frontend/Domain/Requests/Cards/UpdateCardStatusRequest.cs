namespace Domain.Requests.Cards;

public record UpdateCardStatusRequest(
    int CardId, 
    bool IsCompleted);