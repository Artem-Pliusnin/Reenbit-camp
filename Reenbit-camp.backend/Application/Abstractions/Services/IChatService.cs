namespace Application.Abstractions.Services;

public interface IChatService
{
    Task<bool> ImportAsync(
        Stream content,
        string documentId,
        string fileName);

    Task<string> AskAsync(string question);
}