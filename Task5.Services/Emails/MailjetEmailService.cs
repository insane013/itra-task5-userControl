using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace Task5.Services.Emails;

public class MailjetEmailService : IEmailSender
{
    private readonly string? publicApiKey;
    private readonly string? privateApiKey;

    public MailjetEmailService(string? apiKey, string? apiSecret)
    {
        publicApiKey = apiKey;
        privateApiKey = apiSecret;
    }

    public async Task SendEmail(string mailto, string subject, string htmlPart)
    {
        MailjetClient client = new MailjetClient(this.publicApiKey, this.privateApiKey);

        // extract email to json

        MailjetRequest request = new MailjetRequest
        {
            Resource = Send.Resource
        }
        .Property(Send.FromEmail, "marsel092002@gmail.com")
            .Property(Send.FromName, "ITRA TASK-5")
            .Property(Send.Subject, subject)
            .Property(Send.TextPart, "")
            .Property(Send.HtmlPart, htmlPart)
            .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", mailto}
                 }
            });

        var response = await client.PostAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            Console.WriteLine(response.GetData());
            Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
        }
    }
}