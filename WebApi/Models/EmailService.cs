using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace WebApi.Models
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendRegisterEmailAsync(string email);
        Task SendLoginEmailAsync(string email);
    }

    public class EmailService : IEmailService
    {
        private static EmailConfig emailConfig;

        public EmailService()
        {
            emailConfig = new EmailConfig()
            {
                FromName = "Alex's Demo",
                FromAddress = "testemail@website.com",
                Host = "localhost"
            };
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(
                    emailConfig.FromAddress, emailConfig.FromName);
                emailMessage.To.Add(new MailAddress(email, ""));
                emailMessage.Subject = subject;
                emailMessage.IsBodyHtml = true;
                emailMessage.Body = message;

                using (var client = new SmtpClient())
                {
                    client.Host = emailConfig.Host;
                    await client.SendMailAsync(emailMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public async Task SendRegisterEmailAsync(string email)
        {
            string subject = "Registration Successful";
            string message = "Welcome! You have successfully registered.";
            await SendEmailAsync(email, subject, message);
        }

        public async Task SendLoginEmailAsync(string email)
        {
            string subject = "Login Successful";
            string message = "Your login request was successful. Please find " +
                "your authentication token in the HTTP response body.";
            await SendEmailAsync(email, subject, message);
        }
    }
}