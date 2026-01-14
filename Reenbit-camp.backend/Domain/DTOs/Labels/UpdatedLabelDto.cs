namespace Domain.DTOs.Labels;

public class UpdatedLabelDto
{
    public required int BoardId { get; set; }
    
    public LabelDto Label { get; set; }
}