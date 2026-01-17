namespace Domain.Responses.CardAttachments;

public sealed record CardAttachmentDto(
    int Id, 
    string FileUrl, 
    string FileName, 
    string ContentType);