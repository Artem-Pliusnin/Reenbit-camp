namespace Domain.Entities;

public class Session
{
    public int Id { get; set; }
    
    public string Token { get; set; }
    
    public DateTime ExpiresOn { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}