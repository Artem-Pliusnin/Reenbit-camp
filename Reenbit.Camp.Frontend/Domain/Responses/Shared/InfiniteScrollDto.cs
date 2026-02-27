namespace Domain.Responses.Shared;

public sealed record InfiniteScrollDto<T>(
    List<T> Dtos, 
    bool HasMore);