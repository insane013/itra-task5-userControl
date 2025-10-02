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
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.FullName))
            .ForMember(dest => dest.LastLoginTime, opt => opt.MapFrom(x => x.LastLoginTime))
            .ForMember(dest => dest.RegisterTime, opt => opt.MapFrom(x => x.RegisterTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.UserStatus))
            .ForMember(dest => dest.UserPosition, opt => opt.MapFrom(x => x.Position));

        _ = this.CreateMap<User, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.LastLoginTime, opt => opt.MapFrom(x => x.LastLoginTime))
            .ForMember(dest => dest.RegisterTime, opt => opt.MapFrom(x => x.RegisterTime))
            .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(x => x.Status));

        _ = this.CreateMap<UserRegisterDto, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(x => x.UserPosition))
            .ForMember(dest => dest.RegisterTime, opt => opt.MapFrom(x => DateTime.UtcNow))
            .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(x => UserStatus.Unverified));
    }
}
