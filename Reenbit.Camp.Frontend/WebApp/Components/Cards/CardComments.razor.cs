using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace WebApp.Components.Cards;

public partial class CardComments : ComponentBase
{
    private bool IsWritingComment;
    
    private string? InputComment;
    
    private void StartWritingComment()
    {
        InputComment = string.Empty;
        IsWritingComment = true;
    }

    private void SaveComment()
    {
        if (!string.IsNullOrWhiteSpace(InputComment))
        {
            
        }
        
        IsWritingComment = false;
    }

    private void CancelWritingComment()
    {
        IsWritingComment = false;
    }

    private async Task OnTitleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            SaveComment();
        }
        else if (e.Key == "Escape")
        {
            CancelWritingComment();
        }
    }
}