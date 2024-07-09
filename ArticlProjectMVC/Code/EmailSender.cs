using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace ArticlProjectMVC.Code
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("azizaramal36@gmail.com", "~2000aziza2000~"),
                EnableSsl = true,
            };

            return smtpClient.SendMailAsync("azizaramal36@gmail.com", email, subject, htmlMessage);
        }
    }
}
