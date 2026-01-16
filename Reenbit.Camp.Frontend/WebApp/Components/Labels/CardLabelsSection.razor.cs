using System.Text.Json;
using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.Labels;
using Domain.Requests.Labels;
using Domain.Responses.Labels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.Labels;

public partial class CardLabelsSection : ComponentBase, IDisposable
{
    [CascadingParameter(Name="Board")]
    public BoardInfoModel Board { get; set; } = default!;
    
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public int CardId { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<List<CardLabelModel>> OnUpdateLabels { get; set; }
    
    [Inject] 
    private ILabelsService LabelsService { get; set; } = default!;
    
    [Inject] 
    private ICardLabelsService CardLabelsService { get; set; } = default!;
    
    [Inject] 
    private IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    private HubConnection TaskHubConnection;
    
    private List<IDisposable> Subscriptions = new();
    
    private List<CardLabelModel> CardLabels { get; set; } = new();
    
    private List<LabelModel> AvailableLabels { get; set; } = new();
    
    private TelerikPopover? PopoverRef { get; set; }
    
    private bool IsCreateMode = false;
    private string NewLabelText;
    private string NewLabelColor;
    
    private bool IsEditMode = false;
    private LabelModel EditLabel;
    
    protected override async Task OnInitializedAsync()
    {
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
        
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
            await OnUpdateLabels.InvokeAsync(CardLabels);
        }
        
        Subscriptions.Add(HomeHubConnection
            .On<LabelDto>(SubscribeHomeHubConstants.AddNewLabel, AddNewLabel));

        Subscriptions.Add(HomeHubConnection
            .On<LabelDto>(SubscribeHomeHubConstants.UpdateLabel, UpdateLabel));

        Subscriptions.Add(HomeHubConnection
            .On<int>(SubscribeHomeHubConstants.RemoveLabel, RemoveLabel));
        
        Subscriptions.Add(TaskHubConnection
            .On<CardLabelDto>(SubscribeTaskHubConstants.AddCardLabel, AddCardLabel));

        Subscriptions.Add(TaskHubConnection
            .On<CardLabelDto>(SubscribeTaskHubConstants.DeleteCardLabel, RemoveCardLabel));
    }
    
    private bool CheckCardLabelsManagingPermision() => CurrentUser.Role.HasAtLeast(BoardRole.Member);

    private void ResetCreateForm()
    {
        NewLabelText = string.Empty;
        NewLabelColor = "#ffffffff";
    }
    
    private void OpenAddMenu()
    {
        if (CheckCardLabelsManagingPermision())
        {
            IsCreateMode = false;
            ResetCreateForm();
            PopoverRef?.Show();
        }
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
            await OnUpdateLabels.InvokeAsync(CardLabels);
            
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.DeleteCardLabel , 
                    cardLabel, 
                    CardId);
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
            await OnUpdateLabels.InvokeAsync(CardLabels);
            
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.AddCardLabel , 
                    result.Value, 
                    CardId);
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
            
            await HomeHubConnection
                .SendAsync(
                    SendHomeHubConstants.AddNewLabel , 
                    result.Value, 
                    Board.Id);
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
                await OnUpdateLabels.InvokeAsync(CardLabels);
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
            await OnUpdateLabels.InvokeAsync(CardLabels);
            
            await HomeHubConnection
                .SendAsync(
                    SendHomeHubConstants.DeleteLabel, 
                    EditLabel.Id, 
                    Board.Id);
        }
        
        ChangeToBaseMode();
        PopoverRef?.Refresh();
    }
    
    private void AddNewLabel(LabelDto newLabel)
    {
        var labelModel = Mapper.Map<LabelModel>(newLabel);

        if (!AvailableLabels.Any(l => l.Id == labelModel.Id))
        {
            AvailableLabels.Add(labelModel);
        }
        
        PopoverRef?.Refresh();
    }
    
    private void UpdateLabel(LabelDto label)
    {
        var updatedLabel = AvailableLabels.FirstOrDefault(l => l.Id == label.Id);

        if (updatedLabel != null)
        {
            updatedLabel.Text = label.Text;
            updatedLabel.Color = label.Color;
        }
        else
        {
            var updatedLabelModel = CardLabels.FirstOrDefault(l => l.Label.Id == label.Id);

            if (updatedLabelModel != null)
            {
                updatedLabelModel.Label.Text = label.Text;
                updatedLabelModel.Label.Color = label.Color;
            }
        }
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }
    
    private void RemoveLabel(int labelId)
    {
        AvailableLabels.RemoveAll(l => l.Id == labelId);
        CardLabels.RemoveAll(l => l.Label.Id == labelId);
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }

    private void AddCardLabel(CardLabelDto dto)
    {
        var cardLabel = Mapper.Map<CardLabelModel>(dto);
        
        AvailableLabels.RemoveAll(l => l.Id == dto.Label.Id);

        if (!CardLabels.Any(cl => cl.Id == dto.Id))
        {
            CardLabels.Add(cardLabel);
            StateHasChanged();
        }
        
        PopoverRef?.Refresh();
    }
    
    private void RemoveCardLabel(CardLabelDto dto)
    {
        Console.WriteLine(JsonSerializer.Serialize(dto));
        var cardLabel = Mapper.Map<CardLabelModel>(dto);
        Console.WriteLine(JsonSerializer.Serialize(cardLabel));
        
        CardLabels.RemoveAll(cl => cl.Id == dto.Id);
        
        if (!AvailableLabels.Any(l => l.Id == dto.Label.Id))
        {
            AvailableLabels.Add(cardLabel.Label);
        }
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }

    public void Dispose()
    {
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}