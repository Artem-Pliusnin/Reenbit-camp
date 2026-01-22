using Domain.Models.UserAvatars;

namespace Domain.Models.Users;

public class UserModel
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public UserAvatarModel? Avatar { get; set; }
    
    public string Email { get; set; }
}