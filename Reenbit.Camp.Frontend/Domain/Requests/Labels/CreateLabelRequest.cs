namespace Domain.Requests.Labels;

public sealed record CreateLabelRequest( 
    int BoardId, 
    string Text, 
    string Color);