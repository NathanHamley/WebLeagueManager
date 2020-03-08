using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebLeague.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<MailSettings> smtpCredentials)
        {
            Options = smtpCredentials.Value;
        }

        public MailSettings Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options, subject, message, email);
        }

        public async Task Execute(MailSettings mailSettings, string subject, string message, string email)
        {
            await Task.Run(() =>
           {
               var client = new SmtpClient(mailSettings.SmtpServer, mailSettings.SmtpPort);
               client.Credentials = new System.Net.NetworkCredential(mailSettings.Username, mailSettings.Password);
               client.EnableSsl = true;
               var from = new MailAddress(mailSettings.FromAddress);
               var to = new MailAddress(email);
               var mailMsg = new MailMessage(from, to);
               mailMsg.Subject = subject;
               mailMsg.IsBodyHtml = true;
               mailMsg.Body = message;
               client.Send(mailMsg);
           }
            );
        }
    }
}
