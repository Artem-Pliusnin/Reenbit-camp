namespace Domain.DTOs.Cards;

public class UpdatedCardDatesDto
{
    public int CardId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }
}