using Domain.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace WebApp.Pages;

public partial class HomePage : ComponentBase
{
    private BoardsFilterModel filter = new();

    private List<BoardModel> Boards = new()
    {
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
        new BoardModel(){Id = 1, Title = "Project Alpha"},
    };
}