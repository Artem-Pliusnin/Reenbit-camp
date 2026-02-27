namespace Domain.Requests.Cards;

public sealed record CreateCardRequest(
    int ListId, 
    string Title);