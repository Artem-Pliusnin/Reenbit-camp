namespace Domain.Responses.Labels;

public sealed record LabelDto(
    int Id, 
    string Text, 
    string Color);