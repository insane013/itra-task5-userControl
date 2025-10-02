using Task5.Models.User;

namespace Task5.Services.Authentication;

public interface IAuthenticationSerivice
{
    public Task<bool> LoginUser(UserLoginDto model);
    public Task LogOutUser();
    public Task<AuthenticationResult> RegisterUser(UserRegisterDto user, string confirmActionUrl);
    public Task<bool> VerificationComplete(string userId, string token);
}