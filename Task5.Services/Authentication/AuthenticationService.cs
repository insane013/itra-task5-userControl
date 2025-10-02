using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Task5.Database.Entities;
using Task5.Models.User;
using Task5.Services.Verification;

namespace Task5.Services.Authentication;

public class AuthenticationService : BaseService, IAuthenticationSerivice
{
    private readonly UserManager<UserEntity> userManager;
    private readonly IVerificationService verificationService;

    public AuthenticationService(UserManager<UserEntity> userManager, IVerificationService verificationService, IMapper mapper, ILogger<AuthenticationService> logger) 
    : base(mapper, logger)
    {
        this.userManager = userManager;
        this.verificationService = verificationService;
    }

    public async Task RegisterUser(UserRegisterDto model, string confirmActionUrl)
    {
        UserEntity user = this.mapper.Map<UserEntity>(model);
        var res = await this.userManager.CreateAsync(user, model.UserPassword);

        if (res.Succeeded)
        {
            this._logger.LogInformation($"User registration complete..");
            await this.verificationService.Verify(user, confirmActionUrl);
        }
        else
        {
            foreach (var error in res.Errors)
            {
                this._logger.LogWarning($"{error.Description}");
            }
        }
    }

    public Task LoginUser(UserLoginDto user)
    {
        throw new NotImplementedException();
    }
}