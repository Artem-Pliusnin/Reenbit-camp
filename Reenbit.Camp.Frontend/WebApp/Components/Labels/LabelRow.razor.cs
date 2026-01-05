using Domain.Models.Labels;
using Domain.Requests.Labels;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.Labels;

public partial class LabelRow : ComponentBase
{
    [Parameter, EditorRequired] 
    public LabelModel Label { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public EventCallback<LabelModel> OnAdd { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<LabelModel> ObEdit { get; set; }
    private void AddLabel()
    {
        OnAdd.InvokeAsync(Label);
    }
    
    private void EditLabel()
    {
        ObEdit.InvokeAsync(Label);
    }
}