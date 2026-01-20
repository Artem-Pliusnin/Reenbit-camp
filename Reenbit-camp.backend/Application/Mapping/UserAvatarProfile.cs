using AutoMapper;
using Domain.DTOs.UserAvatars;
using Domain.Entities;

namespace Application.Mapping;

public class UserAvatarProfile : Profile
{
    public UserAvatarProfile()
    {
        CreateMap<UserAvatar, UserAvatarDto>();
    }
}