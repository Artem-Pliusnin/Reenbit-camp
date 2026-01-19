namespace Presentation.API.Contracts.CardAttachments;

public class CreateCardAttachmentRequest
{
    public int cardId { get; set; }
    
    public IFormFile file { get; set; }
}