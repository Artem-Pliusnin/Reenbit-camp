using AutoMapper;
using Domain.DTOs.Users;
using Domain.Entities;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.UserName, 
                opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.Password != null));
    }
}