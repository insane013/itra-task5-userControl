using Task5.Models.User;

namespace Task5.Services.Authentication;

public interface IAuthenticationSerivice
{
    public Task LoginUser(UserLoginDto user);
    public Task RegisterUser(UserRegisterDto user, string confirmActionUrl);
}