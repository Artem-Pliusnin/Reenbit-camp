namespace Domain.Requests.Cards;

public sealed record UpdateCardPositionRequest(
    int CardId,
    int NewListId, 
    int NewPosition);