using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Business_Logic_Layer.DTO.EmailDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.Extensions.Configuration;



namespace Business_Logic_Layer.Service.Implementation
{
    public class EmailService(IConfiguration _configuration) : IEmailService
    {
        public async Task SendEmailAsync(string To, string subject, string Msg)
        {
            var emailSettings = new EmailSettingsDTO
            {
                SmtpHost = _configuration["EmailSettings:SmtpHost"] ,
                SmtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]!),
                SmtpUseSSL = bool.Parse(_configuration["EmailSettings:SmtpUseSSL"]!),
                SmtpUser = _configuration["EmailSettings:SmtpUser"]!,
                SmtpPassword = _configuration["EmailSettings:SmtpPassword"],
                FromName = _configuration["EmailSettings:FromName"]!
            };
            string fromMail = emailSettings.SmtpUser;
            string fromPassword = emailSettings.SmtpPassword;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(To));
            message.Body = Msg;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(emailSettings.SmtpHost)
            {
                Port = emailSettings.SmtpPort,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = emailSettings.SmtpUseSSL,
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
