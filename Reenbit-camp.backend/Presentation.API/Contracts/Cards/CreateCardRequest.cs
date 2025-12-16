namespace Presentation.API.Contracts.Cards;

public record CreateCardRequest(
    int ListId, 
    string Title);