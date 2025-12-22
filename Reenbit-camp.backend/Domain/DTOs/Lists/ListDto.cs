using Domain.DTOs.Cards;

namespace Domain.DTOs.Lists;

public class ListDto
{
    public required int Id { get; set; }
    
    public required string Title { get; set; }
    
    public required int Position { get; set; }
    
    public required List<CardDto> Cards { get; set; }
}