using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using LabsWebApp3.Helpers;

namespace LabsWebApp3.Controllers.Helpers
{
    public class EmailService
    {
        public async Task<string> SendEmailAsync(string name, string email, string sendEmail, string subject, string msg)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress($"Администрация сайта - {Config.Name}", sendEmail));
            emailMessage.To.Add(new MailboxAddress(name, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = msg
            };

            using var client = new SmtpClient();

            await client.ConnectAsync("smtp.gmail.com", 587, false);
            if (!client.IsConnected)
                return "Нет соединения с сервером!";
            try
            {
                await client.AuthenticateAsync(Config.Email, Config.EmailPass);
            }
            catch
            {
                return "Север не отвечает!";
            }

            if (!client.IsAuthenticated)
                return "Север не отвечает!";
            try
            {
                await client.SendAsync(emailMessage);
            }
            catch
            {
                return "Нет возможности выслать подтверждение!";
            }
            await client.DisconnectAsync(true);
            return string.Empty;
        }
    }
}