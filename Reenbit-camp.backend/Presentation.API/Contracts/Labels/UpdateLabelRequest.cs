namespace Presentation.API.Contracts.Labels;

public sealed record UpdateLabelRequest(
    int LabelId, 
    string Text, 
    string Color);