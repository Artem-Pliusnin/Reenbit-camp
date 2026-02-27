using Domain.Enums;

namespace Domain.DTOs.Boards;

public sealed record BoardCardDto(
    int Id,
    string Title,
    BoardStatus Status,
    int OwnerId
    );