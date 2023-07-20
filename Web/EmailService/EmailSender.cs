using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Web.EmailService
{
    public class EmailSender
    {
        private const string SUBJECT = "Confirmation email";
        private readonly IOptions<EmailSenderSettings> settings;

        public EmailSender(IOptions<EmailSenderSettings> settings)
        {
            this.settings = settings;
        }

        public async Task SendConfirmationEmailAsync(string email, string link, string username)
        {
            EmailSenderSettings options = settings.Value;
            string apiKey = options.ApiKey;
            SendGridClient client = new SendGridClient(apiKey);

            EmailAddress from = new EmailAddress(options.Email, options.Name);
            EmailAddress to = new EmailAddress(email, username);

            string plainTextContent = "Hello, we are happy to welcome you in eBook Library.";
            string htmlContent = $"Please confirm your email <a href=\"{link}\">here</a>";

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, SUBJECT, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
