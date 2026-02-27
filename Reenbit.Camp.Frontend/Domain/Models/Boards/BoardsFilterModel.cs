namespace Domain.Models.Boards;

public class BoardsFilterModel
{
    public string Title { get; set; } = string.Empty;
    
    public bool OnlyMyBoards { get; set; } = false;
    
    public int Page { get; set; } = 1;
    
    public int PageSize { get; set; } = 12;
}