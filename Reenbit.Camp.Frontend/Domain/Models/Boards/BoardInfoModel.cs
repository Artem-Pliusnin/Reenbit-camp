using Domain.Models.Lists;

namespace Domain.Models.Boards;

public class BoardInfoModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public List<ListModel> Lists { get; set; }
}