using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Task5.Database.Entities;
using Task5.Models.User;
using Task5.Services.Logging;
using Task5.Services.Verification;

namespace Task5.Services.Authentication;

public class AuthenticationService : BaseService, IAuthenticationSerivice
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

    public async Task RegisterUser(UserRegisterDto model, string confirmActionUrl)
    {
        RegistrationLogs.UserRegistrationStart(this._logger, model.Email);

        UserEntity user = this.mapper.Map<UserEntity>(model);
        var res = await this.userManager.CreateAsync(user, model.UserPassword);

        if (this.RegistrationResultProceed(res, model.Email))
        {
            await this.verificationService.Verify(user, confirmActionUrl);
        }
    }

    public async Task<bool> LoginUser(UserLoginDto model)
    {
        var user = await this.userManager.FindByEmailAsync(model.Email);

        if (user is null) return false;

        var result = await this.signInManager.PasswordSignInAsync(user, model.UserPassword, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            user.LastLoginTime = DateTime.UtcNow;
            await this.userManager.UpdateAsync(user);
        }

        return result.Succeeded;
    }

    public async Task LogOutUser()
    {
        await this.signInManager.SignOutAsync();
    }

    private bool RegistrationResultProceed(IdentityResult result, string userEmail)
    {
        if (result.Succeeded)
        {
            RegistrationLogs.UserRegistrationSuccess(this._logger, userEmail);
            return true;
        }

        RegistrationLogs.UserRegistrationFailed(this._logger, userEmail);

        foreach (var error in result.Errors)
        {
            CommonLogs.LogWarningMessage(this._logger, error.Description);
        }

        return false;
    }
}