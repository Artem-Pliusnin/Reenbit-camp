using Domain.Shared;

namespace Domain.Errors;

public static class CardAttachmentErrors
{
    public static readonly Error CreateCardAttachmentError = new(
        "CardAttachment.CreateError",
        "Error occured while creating the card attachment.");
    
    public static readonly Error SavingFileError = new(
        "CardAttachment.SavingFileError",
        "Error occured while saving the file.");
    
    public static readonly Error DeleteCardAttachmentError = new(
        "CardAttachment.DeleteError",
        "Error occured while deleting the card attachment");
}