using MimeKit;
using MailKit.Net.Smtp;

namespace Task5.Services.Emails;

public class GmailSender : IEmailSender
{
    private readonly string? appPassword;
    private readonly string? email;
    private readonly string? senderName;

    public GmailSender(string? gmailMailbox, string? gmailName, string? appPassword)
    {
        this.appPassword = appPassword;
        this.email = gmailMailbox;
        this.senderName = gmailName;
    }

    public async Task SendEmail(string mailto, string subject, string htmlPart)
    {
        var message = this.CombineEmail(mailto, subject, htmlPart);

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587);
            await client.AuthenticateAsync(this.email, this.appPassword);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }

    private MimeMessage CombineEmail(string mailto, string subject, string htmlPart)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(this.senderName, this.email));
        message.To.Add(MailboxAddress.Parse(mailto));

        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = htmlPart
        };

        return message;
    }
}
