using Domain.Models.UserAvatars;

namespace Domain.Models.Users;

public class UserProfileModel
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public UserAvatarModel? Avatar { get; set; }
}
