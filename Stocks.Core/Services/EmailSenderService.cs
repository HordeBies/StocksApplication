using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Stocks.Core.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly string messageFrom;
        private readonly string password;
        public EmailSenderService(IConfiguration config)
        {
            var section = config.GetRequiredSection("EmailSender");
            messageFrom = section["Email"];
            password = section["AppPassword"];
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage()
            {
                From = { new MailboxAddress("BiesStocks", messageFrom)},
                To = { MailboxAddress.Parse(email)},
                Subject = subject,
                Body = new TextPart(TextFormat.Html) { Text = htmlMessage }
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(messageFrom, password);
                client.Send(emailToSend);
                client.Disconnect(true);
            }
            
            return Task.CompletedTask;
        }
    }
}
