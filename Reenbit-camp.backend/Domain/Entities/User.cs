using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? Avatar { get; set; }

    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public UserRole Role { get; set; } = UserRole.User;
    
    public List<BoardMember> Boards { get; set; }
    
    public List<Invitation> ReceivedInvitations { get; set; }
    
    public List<Invitation> SendedInvitations { get; set; }
    
    public List<CardMember> Cards { get; set; }
}