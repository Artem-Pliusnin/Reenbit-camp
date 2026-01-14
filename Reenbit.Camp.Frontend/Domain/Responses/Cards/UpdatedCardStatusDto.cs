namespace Domain.Responses.Cards;

public sealed record UpdatedCardStatusDto(
    int CardId,
    bool IsCompleted);