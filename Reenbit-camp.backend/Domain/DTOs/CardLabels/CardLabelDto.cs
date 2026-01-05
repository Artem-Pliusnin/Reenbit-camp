using Domain.DTOs.Labels;

namespace Domain.DTOs.CardLabels;

public class CardLabelDto
{
    public required int Id { get; set; }
    
    public required LabelDto Label { get; set; }
}