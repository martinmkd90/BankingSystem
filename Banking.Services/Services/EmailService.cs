using Banking.Services.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;

namespace Banking.Services.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Your Bank Name", "no-reply@yourbank.com"));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            client.Connect("smtp.your-email-provider.com", 587, false); // Use your SMTP settings
            client.Authenticate("your-email@example.com", "your-email-password"); // Use your email credentials
            client.Send(emailMessage);
            client.Disconnect(true);
        }
    }

}
