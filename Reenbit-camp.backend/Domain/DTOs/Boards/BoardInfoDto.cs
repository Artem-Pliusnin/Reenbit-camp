using Domain.DTOs.Lists;

namespace Domain.DTOs.Boards;

public class BoardInfoDto
{
    public required int Id { get; set; }
    
    public required string Title { get; set; }
    
    public required List<ListDto> Lists { get; set; }
}