using Domain.Models.Cards;

namespace Domain.Models.Lists;

public class ListModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int Position { get; set; }
    
    public List<CardModel> Cards { get; set; }
}