namespace Presentation.API.Contracts.Labels;

public sealed record CreateLabelRequest( 
    int BoardId, 
    string Text, 
    string Color);