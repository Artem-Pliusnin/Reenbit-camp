namespace Domain.DTOs.Shared;

public class PaginationDto<T>
{
    public List<T> Dtos { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int TotalPages { get; set; }
}