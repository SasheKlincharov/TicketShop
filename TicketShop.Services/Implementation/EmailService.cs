using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using TicketShop.Domain.DomainModels;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        //public async Task<List<OrderEmailMessage>> SendEmailAsync(List<OrderEmailMessage> emailsBatch)
        //{
        //    List<MimeMessage> messagesToSend = new List<MimeMessage>();

        //    foreach (var mail in emailsBatch)
        //    {
        //        var emailMessage = new MimeMessage
        //        {
        //            Sender = new MailboxAddress(_settings.SenderName, _settings.SmtpUserName),
        //            Subject = mail.Subject
        //        };
        //        emailMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SmtpUserName));

        //        emailMessage.Body = new TextPart(TextFormat.Plain) { Text = mail.Content };

        //        emailMessage.To.Add(new MailboxAddress(mail.MailTo, mail.MailTo));

        //        messagesToSend.Add(emailMessage);
        //    }



        //    try
        //    {
        //        using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
        //        {
        //            var socketOption = _settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
        //            await smtpClient.ConnectAsync(_settings.SmtpServer, _settings.SmtpServerPort, socketOption);

        //            if (!string.IsNullOrEmpty(_settings.SmtpUserName))
        //            {
        //                await smtpClient.AuthenticateAsync(_settings.SmtpUserName, _settings.SmtpPassword);
        //            }

        //            for (int i=0; i< messagesToSend.Count; i++)
        //            {
        //                var mail = messagesToSend[i];
        //                await smtpClient.SendAsync(mail);
        //                emailsBatch[i].Status = true;
        //            }

        //            await smtpClient.DisconnectAsync(true);
        //        }

        //    }
        //    catch (SmtpException ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //    }

        //    return emailsBatch;
        //}
    }
}
