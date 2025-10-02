using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Task5.Database.Entities;
using Task5.Services.Emails;

namespace Task5.Services.Verification;

public class EmailVerificationService : BaseService, IVerificationService
{
    private readonly IEmailSender emailSender;
    private readonly UserManager<UserEntity> userManager;

    public EmailVerificationService(IEmailSender emailSender, UserManager<UserEntity> userManager, IMapper mapper, ILogger<EmailVerificationService> logger)
    : base(mapper, logger)
    {
        this.emailSender = emailSender;
        this.userManager = userManager;
    }

    public async Task Verify(UserEntity user, string confirmActionUrl)
    {
        if (user.Email is null) throw new ArgumentException("Can't verify user without email.");

        this._logger.LogInformation($"User verification started..");

        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

        var url = this.GenerateConfirmationLink(confirmActionUrl, user.Id, token);

        this._logger.LogInformation($"Generated link: {confirmActionUrl}");

        await this.SendConfirmationEmail(user.Email, url);
    }

    private async Task SendConfirmationEmail(string email, string confirmationLink)
    {
        string text = $"To confirm your email please follow this link: {confirmationLink}";
        string html = $@"To confirm your email please follow this link: <a href='{confirmationLink}'>{confirmationLink}</a>";
        string subject = "Task-5 app confirmation email";

        await this.emailSender.SendEmail(email, subject, html);
    }

    private string GenerateConfirmationLink(string confirmActionUrl, string userId, string token)
    {
        return $"{confirmActionUrl}?userId={userId}&token={token}";
    }
}
