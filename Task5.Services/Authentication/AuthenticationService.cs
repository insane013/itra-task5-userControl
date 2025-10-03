using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Task5.Database.Entities;
using Task5.Models.User;
using Task5.Services.Exceptions;
using Task5.Services.Logging;
using Task5.Services.Verification;

namespace Task5.Services.Authentication;

public class AuthenticationService : BaseService, IAuthenticationService
{
    private readonly UserManager<UserEntity> userManager;
    private readonly SignInManager<UserEntity> signInManager;
    private readonly IVerificationService verificationService;

    public AuthenticationService(UserManager<UserEntity> userManager, IVerificationService verificationService,
                                 SignInManager<UserEntity> signInManager,
                                 IMapper mapper, ILogger<AuthenticationService> logger)
    : base(mapper, logger)
    {
        this.userManager = userManager;
        this.verificationService = verificationService;
        this.signInManager = signInManager;
    }

    public async Task<AuthenticationResult> RegisterUser(UserRegisterDto model, string confirmActionUrl)
    {
        RegistrationLogs.UserRegistrationStart(this._logger, model.Email);

        UserEntity user = this.mapper.Map<UserEntity>(model);
        var res = await this.userManager.CreateAsync(user, model.UserPassword);

        return await this.RegistrationResultProceed(res, model.Email, confirmActionUrl);
    }

    public async Task<bool> LoginUser(UserLoginDto model)
    {
        var user = await this.userManager.FindByEmailAsync(model.Email);

        if (user is null) return false;

        var result = await this.signInManager.PasswordSignInAsync(user, model.UserPassword, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded) await this.UpdateUserLoginTime(user);

        return result.Succeeded;
    }

    public async Task LogOutUser()
    {
        await this.signInManager.SignOutAsync();
    }

    public async Task<bool> VerificationComplete(string userId, string token)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        this._logger.LogInformation($"UserId: {userId}\n\tToken: {decodedToken}\n\tEncoded: {token}");

        if (user is not null)
        {
            var result = await this.userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded) await this.UpdateUserStatus(user, UserStatus.Active);

            return result.Succeeded;
        }

        return false;
    }

    private async Task UpdateUserStatus(UserEntity user, UserStatus status)
    {
        if (user.UserStatus != (int)UserStatus.Blocked)
        {
            user.UserStatus = (int)status;
            await this.userManager.UpdateAsync(user);
        }
    }

    private async Task UpdateUserLoginTime(UserEntity user)
    {
        user.LastLoginTime = DateTime.UtcNow;
        await this.userManager.UpdateAsync(user);
    }

    private async Task<AuthenticationResult> RegistrationResultProceed(IdentityResult result, string userEmail, string confirmActionUrl)
    {
        return result.Succeeded ? await this.RegistractionSuccess(userEmail, confirmActionUrl) : this.RegistractionFail(result, userEmail);
    }
    
    private async Task<AuthenticationResult> RegistractionSuccess(string userEmail, string confirmActionUrl)
    {
        RegistrationLogs.UserRegistrationSuccess(this._logger, userEmail);

        var createdUser = await this.userManager.FindByEmailAsync(userEmail);
        await this.verificationService.Verify(createdUser!, confirmActionUrl);

        await this.signInManager.SignInAsync(createdUser!, true);
        await this.UpdateUserLoginTime(createdUser!);

        return AuthenticationResult.Success;
    }

    private AuthenticationResult RegistractionFail(IdentityResult result, string userEmail)
    {
        RegistrationLogs.UserRegistrationFailed(this._logger, userEmail);

        AuthenticationResult response = new AuthenticationResult { Succeeded = false };

        foreach (var error in result.Errors)
        {
            CommonLogs.LogWarningMessage(this._logger, error.Description);

            response.Errors.Append(error.Description);
        }

        return response;
    }
}