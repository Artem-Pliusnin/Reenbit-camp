using Domain.Models.CardMembers;
using Domain.Models.Labels;

namespace Domain.Models.Cards;

public class CardModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int Position { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public List<CardLabelModel> Labels { get; set; }
    
    public List<CardMemberModel> Members { get; set; }
}