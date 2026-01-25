using Domain.Enums;
using Domain.Models.FAQChat;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace WebApp.Components.FAQChat;

public partial class FAQChat : ComponentBase
{
    private string InputMessage = string.Empty;

    private List<MessageModel> Messages = new();

    protected override async Task OnInitializedAsync()
    {
        
    }

    async Task SendMessage()
    {
    }
}