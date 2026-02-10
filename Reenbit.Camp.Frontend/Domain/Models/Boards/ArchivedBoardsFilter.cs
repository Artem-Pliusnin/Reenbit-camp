namespace Domain.Models.Boards;
public class ArchivedBoardsFilter
{
    public string? Title { get; set; } = String.Empty;
    
    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 12;
};