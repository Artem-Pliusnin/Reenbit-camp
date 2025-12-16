namespace Presentation.API.Contracts.Cards;

public sealed record UpdateCardPositionRequest(
    int CardId,
    int NewListId, 
    int NewPosition);