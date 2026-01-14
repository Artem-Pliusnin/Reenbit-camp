using System.Text.Json;
using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Enums;
using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.Labels;
using Domain.Requests.Boards;
using Domain.Requests.Labels;
using Domain.Responses.BoardMembers;
using Domain.Responses.Invitations;
using Domain.Responses.Labels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Pages;

public partial class BoardPage : ComponentBase, IAsyncDisposable
{
    [Parameter]
    public int BoardId { get; set; }
    
    [Inject] 
    public IMapper Mapper { get; set; } = default!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    
    [Inject] 
    public IBoardsService BoardsService { get; set; } = default!;
    
    [Inject] 
    public IBoardMembersService BoardMemberService { get; set; } = default!;
    
    [Inject] 
    public ILabelsService LabelsService { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    
    private List<IDisposable> Subscriptions = new();

    private BoardInfoModel Board = new();
    
    private List<LabelModel> Labels = new();

    private BoardMemberModel CurrentBoardMember = new();

    private bool isLoading;
    
    private bool IsEditingTitle;
    
    private bool IsMembersDialogOpen;
    
    private string TitleInput = string.Empty;
    
    private TelerikPopover? PopoverRef { get; set; }
    
    private bool IsCreateMode = false;
    private string NewLabelText;
    private string NewLabelColor;
    
    private bool IsEditMode = false;
    private LabelModel EditLabel;
    
    protected override async Task OnParametersSetAsync()
    {
        isLoading = true;
        
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
            
        var result = await LoadBoardData();

        if (result)
        {
            await HomeHubConnection.SendAsync(SendHomeHubConstants.AddToBoardGroup, BoardId);
            
            Subscriptions.Add(HomeHubConnection
                .On<string>(SubscribeHomeHubConstants.UpdateBoardTitle, UpdateBoardTitle));

            Subscriptions.Add(HomeHubConnection
                .On<LabelDto>(SubscribeHomeHubConstants.AddNewLabel, AddNewLabel));

            Subscriptions.Add(HomeHubConnection
                .On<LabelDto>(SubscribeHomeHubConstants.UpdateLabel, UpdateLabel));

            Subscriptions.Add(HomeHubConnection
                .On<int>(SubscribeHomeHubConstants.RemoveLabel, RemoveLabel));
            
            Subscriptions.Add(HomeHubConnection
                .On<int>(SubscribeHomeHubConstants.DeletedMember, OnDeletedMember));
            
            Subscriptions.Add(HomeHubConnection
                .On<UpdatedCardMemberRoleDto>(SubscribeHomeHubConstants.UpdateMemberRole, OnUpdateMemberRole));
        }
    }

    private async Task OnDeletedMember(int memberId)
    {
        if (CurrentBoardMember.Id == memberId)
        {
            NavigationManager.NavigateTo($"/");
        }
        await LoadBoardData();
        StateHasChanged();
    }
    
    private void OnUpdateMemberRole(UpdatedCardMemberRoleDto dto)
    {
        if (CurrentBoardMember.Id == dto.MemberId)
        {
            CurrentBoardMember.Role = (BoardRole)dto.RoleId;
            StateHasChanged();
        }
    }
    
    private async Task<bool> LoadBoardData()
    {
        isLoading = true;
        var boardResult = await BoardsService.GetInfoAsync(BoardId);

        if (boardResult.IsSuccess)
        {
            Board = boardResult.Value;
        }
        
        var memberResult = await BoardMemberService.GetCurrentAsync(BoardId);
        
        if (memberResult.IsSuccess)
        {
            CurrentBoardMember = memberResult.Value;
        }
        else
        {
            NavigationManager.NavigateTo($"/");
            return false;
        }
        
        var labelsResult = await LabelsService.GetByBoardAsync(BoardId);
        
        if (labelsResult.IsSuccess)
        {
            Labels = labelsResult.Value;
        }
        
        isLoading = false;
        return true;
    }
    
    private void OpenMembersDialog()
    {
        IsMembersDialogOpen = true;
    }
    
    private void CloseMembersDialog()
    {
        IsMembersDialogOpen = false;
    }

    private void StartEditTitle()
    {
        TitleInput = Board.Title;
        IsEditingTitle = true;
    }

    private async Task SaveTitle()
    {
        if (!string.IsNullOrWhiteSpace(TitleInput))
        {
            Board.Title = TitleInput;

            await BoardsService.UpdateAsync(
                BoardId, 
                new UpdateBoardRequest(BoardId, TitleInput));
        }
        
        IsEditingTitle = false;
    }

    private void CancelEdit()
    {
        IsEditingTitle = false;
    }

    private async Task OnTitleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SaveTitle();
        }
        else if (e.Key == "Escape")
        {
            CancelEdit();
        }
    }

    private async Task UpdateBoardTitle(string newTitle)
    {
        Board.Title = newTitle;
        
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task AddNewLabel(LabelDto newLabel)
    {
        var labelModel = Mapper.Map<LabelModel>(newLabel);

        if (!Labels.Any(l => l.Id == labelModel.Id))
        {
            Labels.Add(labelModel);
        }
        
        PopoverRef?.Refresh();
    }
    
    private async Task UpdateLabel(LabelDto label)
    {
        var updatedLabel = Labels.FirstOrDefault(l => l.Id == label.Id);

        if (updatedLabel != null)
        {
            updatedLabel.Text = label.Text;
            updatedLabel.Color = label.Color;
        }
        
        await LoadBoardData();
        StateHasChanged();
        PopoverRef?.Refresh();
    }
    
    private async Task RemoveLabel(int labelId)
    {
        Labels.RemoveAll(l => l.Id == labelId);
        
        await LoadBoardData();
        StateHasChanged();
        PopoverRef?.Refresh();
    }
    
    private void ResetCreateForm()
    {
        NewLabelText = string.Empty;
        NewLabelColor = "#ffffffff";
    }
    
    private void OpenLabelsMenu()
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
    
    private async Task CreateLabel()
    { 
        var result = await LabelsService
            .CreateAsync(
                new CreateLabelRequest(
                    Board.Id, 
                    NewLabelText, 
                    NewLabelColor));

        if (result.IsSuccess)
        {
            Labels.Add(result.Value);
            await HomeHubConnection
                .SendAsync(SendHomeHubConstants.AddNewLabel , result.Value, BoardId);
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
            var label = Labels.FirstOrDefault(l => l.Id == EditLabel.Id);
            if (label is not null)
            {
                label.Text = EditLabel.Text;
                label.Color = EditLabel.Color;
            }
        }

        ChangeToBaseMode();
        PopoverRef?.Refresh();
        await LoadBoardData();
    }
    
    private async Task DeleteLabel()
    {
        var result = await LabelsService.DeleteAsync(EditLabel.Id);

        if (result.IsSuccess)
        {
            Labels.RemoveAll(l => l.Id == EditLabel.Id);
            await HomeHubConnection
                .SendAsync(SendHomeHubConstants.DeleteLabel, EditLabel.Id, BoardId);
        }
        
        ChangeToBaseMode();
        PopoverRef?.Refresh();
        await LoadBoardData();
    }

    public async ValueTask DisposeAsync()
    {
        await HomeHubConnection.SendAsync(SendHomeHubConstants.DeleteFromBoardGroup, BoardId);

        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}