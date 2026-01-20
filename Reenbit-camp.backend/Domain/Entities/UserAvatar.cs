namespace Domain.Entities;

public class UserAvatar
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public string FileName { get; set; }
    
    public string FileUrl { get; set; }
    
    
    public User User { get; set; }
}