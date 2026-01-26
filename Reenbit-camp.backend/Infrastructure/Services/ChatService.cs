using Application.Abstractions.Services;
using Domain.Constants.FIleConstants;
using Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly Dictionary<int, ChatHistory> _sessions;
    private readonly Kernel _kernel;
    private readonly IKernelMemory _memory;

    public ChatService(
        IOptions<AzureOpenAISettings> openAIOptions,
        IKernelMemory memory)
    {
        _sessions = new Dictionary<int, ChatHistory>();
        
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
    
    public void StartChatSession(int userId)
    {
        _sessions[userId] = new ChatHistory();
    }

    public bool EndChatSession(int userId)
    {
        return _sessions.Remove(userId);
    }

    public async Task<string> AskAsync(int userId ,string question)
    {
        OpenAIPromptExecutionSettings settings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        };
        
        if (!_sessions.ContainsKey(userId))
        {
            StartChatSession(userId);
        }
        
        var chatHistory = _sessions[userId];
        
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        
        var prompt = $@"
            Question to Kernel Memory: {question}

            Kernel Memory Answer: {{{{memory.ask question=""{question}"" index=""{FAQConstants.FAQIndex}""}}}}

            If the answer is empty say 'I don't know',otherwise reply with the answer.
            ";
    
        chatHistory.AddMessage(AuthorRole.User, prompt);
        
        var result = await chatCompletionService
            .GetChatMessageContentAsync(chatHistory, settings, _kernel);

        chatHistory.AddMessage(AuthorRole.Assistant, result.Content);

        return result.Content;
    }
}