namespace Domain.Requests.Labels;

public sealed record CreateCardLabelRequest(int CardId, int LabelId);