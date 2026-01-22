using AutoMapper;
using Domain.Models.UserAvatars;
using Domain.Responses.UserAvatars;

namespace Services.Mapping;

public class UserAvatarProfile : Profile
{
    public UserAvatarProfile()
    {
        CreateMap<UserAvatarDto, UserAvatarModel>().ReverseMap();
    }
}