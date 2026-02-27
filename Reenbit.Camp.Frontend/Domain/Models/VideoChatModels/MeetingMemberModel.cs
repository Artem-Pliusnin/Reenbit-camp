using Domain.Models.Users;

namespace Domain.Models.VideoChatModels;

public class MeetingMemberModel
{
    public UserModel User { get; set; }
    
    public bool IsVideoActive { get; set; }
    
    public bool IsAudioActive { get; set; }
}