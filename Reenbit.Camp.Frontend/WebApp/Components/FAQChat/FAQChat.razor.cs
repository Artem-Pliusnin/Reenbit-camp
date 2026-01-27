using Domain.Enums;
using Domain.Models.FAQChat;
using Domain.Requests.FAQChat;
using Domain.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;

namespace WebApp.Components.FAQChat;

public partial class FAQChat : ComponentBase, IAsyncDisposable
{
    [Inject] 
    public IFAQChatService FaqChatService { get; set; } = default!;
    
    private string InputMessage = string.Empty;

    private List<MessageModel> Messages = new();

    private bool isResponseLoading;

    protected override async Task OnInitializedAsync()
    {
        await FaqChatService.StartSessionAsync();
        
        Messages.Add(
            new MessageModel()
            {
                Sender = SenderType.Bot,
                Message = "How can i help you?)",
            });
    }

    async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(InputMessage))
        {
            return;
        }
        
        Messages.Add(
            new MessageModel()
            {
                Sender = SenderType.User,
                Message = InputMessage,
            });
        
        isResponseLoading = true;
        
        var response = await FaqChatService
            .AskQuestionAsync(new AskQuestionRequest(InputMessage));

        if (response.IsSuccess)
        {
            Messages.Add(response.Value);
            isResponseLoading = false;
        }
        
        InputMessage = string.Empty;
    }

    public async ValueTask DisposeAsync()
    {
        await FaqChatService.EndSessionAsync();
    }
}