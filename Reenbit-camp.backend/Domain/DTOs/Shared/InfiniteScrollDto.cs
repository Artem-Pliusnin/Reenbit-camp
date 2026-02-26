namespace Domain.DTOs.Shared;

public class InfiniteScrollDto<T>
{
    public List<T> Dtos { get; set; }
    
    public bool HasMore { get; set; }
}