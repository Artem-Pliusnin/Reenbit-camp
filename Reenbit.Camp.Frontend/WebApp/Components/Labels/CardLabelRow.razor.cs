using Domain.Models.Labels;
using Domain.Requests.Labels;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.Labels;

public partial class CardLabelRow : ComponentBase
{
    [Parameter, EditorRequired] 
    public CardLabelModel CardLabel { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public EventCallback<CardLabelModel> OnDelete { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<LabelModel> ObEdit { get; set; }
    
    [Inject]
    private ILabelsService LabelsService { get; set; } = default!;
    
    private void DeletLabel()
    {
        OnDelete.InvokeAsync(CardLabel);
    }
    
    private void EditLabel()
    {
        ObEdit.InvokeAsync(CardLabel.Label);
    }
    
}