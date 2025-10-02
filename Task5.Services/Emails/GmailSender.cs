using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using MimeKit;
using MailKit.Net.Smtp;

namespace Task5.Services.Emails;

public class GmailSender : IEmailSender
{
    private readonly string? appPassword;

    public GmailSender(string? appPassword)
    {
        this.appPassword = appPassword;
    }

    public async Task SendEmail(string mailto, string subject, string htmlPart)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Itra Task-5 <noreply@myapp.com>", "marsel092002@gmail.com"));
        message.To.Add(MailboxAddress.Parse(mailto));

        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = htmlPart
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587);
            await client.AuthenticateAsync("marsel092002@gmail.com", this.appPassword);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
