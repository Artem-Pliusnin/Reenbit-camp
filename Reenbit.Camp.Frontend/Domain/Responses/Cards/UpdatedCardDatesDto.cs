namespace Domain.Responses.Cards;

public record UpdatedCardDatesDto(
    int CardId,  
    DateTime? StartDate, 
    DateTime? DueDate);