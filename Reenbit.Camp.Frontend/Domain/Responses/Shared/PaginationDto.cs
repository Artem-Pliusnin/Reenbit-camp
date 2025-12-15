namespace Domain.Responses.Shared;

public sealed record PaginationDto<T>(
    List<T> Dtos, 
    int CurrentPage, 
    int TotalPages);