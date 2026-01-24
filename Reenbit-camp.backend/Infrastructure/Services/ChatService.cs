using Application.Abstractions.Services;
using Domain.Constants.FIleConstants;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly Kernel _kernel;
    private readonly IKernelMemory _memory;

    public ChatService(
        IOptions<AzureOpenAISettings> openAIOptions,
        IKernelMemory memory)
    {
        _memory = memory;
        var openAI = openAIOptions.Value;

        _kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: openAI.DeploymentChatName,
                endpoint: openAI.Endpoint,
                apiKey: openAI.ApiKey)
            .Build();
        
        var memoryPlugin = new MemoryPlugin(
            _memory,
            waitForIngestionToComplete: true);

        _kernel.ImportPluginFromObject(memoryPlugin, "memory");
    }

    public async Task<bool> ImportAsync(Stream content, string documentId, string fileName)
    {
        try
        {
            await _memory.ImportDocumentAsync(
                content: content, 
                documentId: documentId, 
                fileName: fileName,
                index: FAQConstants.FAQIndex);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public async Task<bool> RemoveAsync(string documentId)
    {
        try
        {
            await _memory.DeleteDocumentAsync(documentId: documentId, index: FAQConstants.FAQIndex);
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> AskAsync(string question)
    {
        OpenAIPromptExecutionSettings settings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        };

        var chatHistory = new ChatHistory();
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        
        var memoryResult = await _memory.AskAsync(
            question: question,
            index: FAQConstants.FAQIndex);
        
        var prompt = $@"
           Question to Kernel Memory: {question}

           Kernel Memory Answer:{memoryResult.Result}

           If the answer is empty say 'I don't know', otherwise reply with the answer.
           ";
        
        chatHistory.AddMessage(AuthorRole.User, prompt);
        
        var result = await chatCompletionService.GetChatMessageContentAsync(chatHistory, settings, _kernel);

        chatHistory.AddMessage(AuthorRole.Assistant, result.Content);

        return result.Content;
    }
}