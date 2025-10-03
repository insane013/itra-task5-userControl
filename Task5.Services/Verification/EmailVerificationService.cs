using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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

        var url = await this.GenerateConfirmationLinkAsync(confirmActionUrl, user);

        await this.SendConfirmationEmail(user.Email, url);
    }

    private async Task SendConfirmationEmail(string email, string confirmationLink)
    {
        string html = $@"To confirm your email please follow this link: <a href='{confirmationLink}'>{confirmationLink}</a>";
        string subject = "Task-5 app confirmation email";

        await this.emailSender.SendEmail(email, subject, html);
    }

    private async Task<string> GenerateConfirmationLinkAsync(string confirmActionUrl, UserEntity user)
    {
        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        this._logger.LogInformation($"UserId: {user.Id}\n\tToken: {token}\n\tEncoded: {encodedToken}");

        return $"{confirmActionUrl}?userId={user.Id}&token={encodedToken}";
    }
}
