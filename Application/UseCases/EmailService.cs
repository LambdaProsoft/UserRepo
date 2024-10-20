using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationCode(string toEmail, string verificationCode)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);
            var smtpUsername = _configuration["Smtp:Username"];
            var smtpPassword = _configuration["Smtp:Password"];
            var enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]);

            var fromEmail = smtpUsername; 

            var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Código de verificación",
                Body = $"Tu código de verificación es: {verificationCode}",
                IsBodyHtml = false
            };

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
