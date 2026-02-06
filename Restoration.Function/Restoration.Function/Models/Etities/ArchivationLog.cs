namespace Restoration.Function.Models.Etities;

public class ArchivationLog
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    
    public int boardId { get; set; }
    
    public int statusId { get; set; }
    
    public string statusName { get; set; }
    
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
}