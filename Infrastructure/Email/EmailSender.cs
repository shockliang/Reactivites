using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Email
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string userEmail, string emailSubject, string msg)
        {
            var client = new SendGridClient(_configuration["SendGrid:Key"]);
            var message = new SendGridMessage
            {
                From = new EmailAddress(_configuration["SendGrid:UserEmail"], _configuration["SendGrid:User"]),
                Subject = emailSubject,
                PlainTextContent = msg,
                HtmlContent = msg
            };
            
            message.AddTo(new EmailAddress(userEmail));
            message.SetClickTracking(false, false);
            await client.SendEmailAsync(message);
        }
    }
}