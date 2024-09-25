using Core.Interfaces;
using Core.Models.Utility;

using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;


namespace Core.Services
{
    public class SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger) : IEmailSender
    {
        private readonly MailSettings mailSettings = _mailSettings.Value;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
#if DEBUG
            email = "duongnt115@fpt.edu.vn";
#endif
            var message = new MimeMessage
            {
                Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail),
                Subject = subject
            };
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = builder.ToMessageBody();

            await SendmailAsync(message);

        }
        public async Task SendEmailAsync(string email, string subject, BodyBuilder builder)
        {
#if DEBUG
            email = "duongnt115@fpt.edu.vn";
#endif
            var message = new MimeMessage
            {
                Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail),
                Subject = subject
            };
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));

            message.Body = builder.ToMessageBody();

           await SendmailAsync(message);

        }

        private async Task SendmailAsync(MimeMessage message)
        {
#if DEBUG
            message.To.Clear();
            message.To.Add(MailboxAddress.Parse("duongnt115@fpt.edu.vn"));
#endif
            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);

                _logger.LogInformation("Send mail to: " + message.To.Mailboxes.FirstOrDefault()?.Address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi gửi mail");
            }

            smtp.Disconnect(true);
        }
    }
}
