namespace Archivation.Function.Models.Etities;

public class CardMember
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public int UserId { get; set; }
    
    public Card Card { get; set; }
}