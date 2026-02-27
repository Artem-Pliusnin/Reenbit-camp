using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Boards;

public class NewBoardModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
}