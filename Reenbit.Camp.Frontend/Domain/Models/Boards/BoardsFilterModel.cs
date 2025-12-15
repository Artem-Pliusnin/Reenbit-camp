namespace Domain.Models.Boards;

public class BoardsFilterModel
{
    public string Title { get; set; } = string.Empty;
    
    public bool OnlyMyBoards { get; set; } = false;
}