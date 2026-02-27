namespace Domain.Models.Cards;

public class CardsFilterModel
{
    public string? Title { get; set; }
    
    public int BoardId {get; set;}
    
    public bool OnlyAssignedToUser {get; set;}
    
    public int Page { get; set; }
    
    public int PageSize { get; set; }
};