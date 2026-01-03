using Domain.Models.Boards;
using Domain.Models.Labels;
using Domain.Requests.Labels;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;

namespace WebApp.Components.Labels;

public partial class CardLabelsSection : ComponentBase
{
    [CascadingParameter]
    public BoardInfoModel Board { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public int CardId { get; set; }

    private List<CardLabelModel> CardLabels { get; set; } = new();
    
    private List<LabelModel> AvailableLabels { get; set; } = new();
    
    [Inject] 
    private ILabelsService LabelsService { get; set; } = default!;
    
    [Inject] 
    private ICardLabelsService CardLabelsService { get; set; } = default!;
    
    private TelerikPopover? PopoverRef { get; set; }
    
    private bool IsCreateMode = false;
    private string NewLabelText;
    private string NewLabelColor;
    
    private bool IsEditMode = false;
    private LabelModel EditLabel;

    private void ResetCreateForm()
    {
        NewLabelText = string.Empty;
        NewLabelColor = "#ffffff";
    }
    
    private void OpenAddMenu()
    {
        IsCreateMode = false;
        ResetCreateForm();
        PopoverRef?.Show();
    }
    
    private void ClosAddMenu()
    {
        ChangeToBaseMode();
        PopoverRef?.Hide();
    }

    private void ChangeToCreateMode()
    {
        IsCreateMode = true;
        PopoverRef?.Refresh();
    }
    
    private void ChangeToEditMode(LabelModel label)
    {
        EditLabel = new LabelModel
        {
            Id = label.Id, 
            Text = label.Text, 
            Color = label.Color
        } ;
        
        IsEditMode = true;
        PopoverRef?.Refresh();
    }
    
    private void ChangeToBaseMode()
    {
        IsCreateMode = false;
        IsEditMode = false;
        PopoverRef?.Refresh();
    }
    
    private void ColorPickerOnChange(object args)
    {
        PopoverRef?.Refresh();
    }

    private async Task DeleteCardLabel(CardLabelModel cardLabel)
    {
        var result = await CardLabelsService
            .DeleteAsync(cardLabel.Id);

        if (result.IsSuccess)
        {
            CardLabels.Remove(cardLabel);
            AvailableLabels.Add(cardLabel.Label);
        }
        
        PopoverRef?.Refresh();
    }

    private async Task AddLabel(LabelModel label)
    {
        var result = await CardLabelsService
            .CreateAsync(new CreateCardLabelRequest(CardId, label.Id));

        if (result.IsSuccess)
        {
            AvailableLabels.Remove(label);
            CardLabels.Add(result.Value);
        }
        
        PopoverRef?.Refresh();
    }
    
    private async Task CreateLabel()
    { 
        var result = await LabelsService
            .CreateAsync(new CreateLabelRequest(Board.Id, NewLabelText, NewLabelColor));

        if (result.IsSuccess)
        {
            AvailableLabels.Add(result.Value);
        }
        
        ResetCreateForm();
        IsCreateMode = false;
        
        PopoverRef?.Refresh();
    }

    private async Task SaveEditedLabel()
    {
        var result = await LabelsService.UpdateAsync(
            EditLabel.Id, 
            new UpdateLabelRequest(
                EditLabel.Id, 
                EditLabel.Text, 
                EditLabel.Color));

        if (result.IsSuccess)
        {
            var cardLabel = CardLabels.FirstOrDefault(cl => cl.Label.Id == EditLabel.Id);
            if (cardLabel is not null)
            {
                cardLabel.Label.Text = EditLabel.Text;
                cardLabel.Label.Color = EditLabel.Color;
            }

            var availableLabel = AvailableLabels.FirstOrDefault(l => l.Id == EditLabel.Id);
            if (availableLabel is not null)
            {
                availableLabel.Text = EditLabel.Text;
                availableLabel.Color = EditLabel.Color;
            }
        }

        ChangeToBaseMode();
        PopoverRef?.Refresh();
    }
    
    private async Task DeleteLabel()
    {
        var result = await LabelsService.DeleteAsync(EditLabel.Id);

        if (result.IsSuccess)
        {
            CardLabels.RemoveAll(cl => cl.Label.Id == EditLabel.Id);
            AvailableLabels.RemoveAll(l => l.Id == EditLabel.Id);
        }
        
        ChangeToBaseMode();
        PopoverRef?.Refresh();
    }
    

    protected override async Task OnInitializedAsync()
    {
        var availableLabelsResult = await LabelsService
            .GetNotConnectedAsync(CardId);

        if (availableLabelsResult.IsSuccess)
        {
            AvailableLabels = availableLabelsResult.Value;
        }
        
        var cardLabelsResult = await CardLabelsService
            .GetByCardAsync(CardId);

        if (cardLabelsResult.IsSuccess)
        {
            CardLabels = cardLabelsResult.Value;
        }
    }
}