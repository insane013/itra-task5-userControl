namespace Task5.Services.Emails;

public interface IEmailSender
{
    public Task SendEmail(string mailto, string subject, string htmlPart);
}
