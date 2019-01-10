using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EAD.CRM.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly string SMTPServer = "";
        private readonly string Username = "";
        private readonly string Password = "";
        private readonly int Port = 587;
        private readonly string FromEmail = "";
        public EmailService()
        {
            //Populate the fields from config file.
        }
        public async Task SendEmail(string toAddress, string subject, string htmlContent)
        {
            var client = new SmtpClient(SMTPServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Username, Password);

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(FromEmail);
                mailMessage.To.Add(toAddress);
                mailMessage.Body = htmlContent;
                mailMessage.Subject = subject;
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}