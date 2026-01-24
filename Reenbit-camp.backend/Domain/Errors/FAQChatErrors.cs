using Domain.Shared;

namespace Domain.Errors;

public class FAQChatErrors
{
    public static readonly Error ResponseError = new(
        "FAQ.ResponseError",
        "Error occured while processing your request.");
    
    public static readonly Error FileImportError = new(
        "FAQ.FileImportError",
        "Error occured while importing the file.");
}