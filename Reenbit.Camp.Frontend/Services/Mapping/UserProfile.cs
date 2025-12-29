using AutoMapper;
using Domain.Models.Users;
using Domain.Responses.Users;

namespace Services.Mapping;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserModel>();
    }
}