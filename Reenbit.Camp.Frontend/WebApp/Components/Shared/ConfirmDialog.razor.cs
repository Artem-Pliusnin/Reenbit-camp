using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Shared;

public partial class ConfirmDialog : ComponentBase
{
    [Parameter, EditorRequired]
    public bool IsOpen { get; set; }
    
    [Parameter, EditorRequired]
    public string Title { get; set; }
    
    [Parameter, EditorRequired]
    public string Text { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback OnConfirm { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback OnCancel { get; set; }

    private async Task Cancel()
    {
        await OnConfirm.InvokeAsync();
    }

    private async Task Confirm()
    {
        await OnConfirm.InvokeAsync();
    }
}