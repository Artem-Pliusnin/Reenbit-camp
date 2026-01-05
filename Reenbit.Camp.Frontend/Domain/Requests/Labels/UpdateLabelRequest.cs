namespace Domain.Requests.Labels;

public sealed record UpdateLabelRequest(
    int LabelId, 
    string Text, 
    string Color);