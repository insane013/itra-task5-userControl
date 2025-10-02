using AutoMapper;
using Task5.Database.Entities;
using Task5.Models.User;

namespace Task5.WebApi.Services.Mappers;

public class UserMapper: Profile
{
    public UserMapper()
    {
        _ = this.CreateMap<UserEntity, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.LastLoginTime, opt => opt.MapFrom(x => x.lastLoginTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.userStatus))
            .ForMember(dest => dest.UserPosition, opt => opt.Ignore()); // Add mapping for postion

        _ = this.CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.lastLoginTime, opt => opt.MapFrom(x => x.LastLoginTime))
            .ForMember(dest => dest.userStatus, opt => opt.MapFrom(x => x.Status));

        _ = this.CreateMap<UserRegisterDto, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.userStatus, opt => opt.MapFrom(x => UserStatus.Unverified));
    }
}
