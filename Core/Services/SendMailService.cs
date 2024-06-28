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

            _logger.LogInformation("Send mail to: " + email);

        }
        public async Task SendEmailAsync(string email, string subject, BodyBuilder builder)
        {
            var message = new MimeMessage
            {
                Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail),
                Subject = subject
            };
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));

            message.Body = builder.ToMessageBody();

           await SendmailAsync(message);

            _logger.LogInformation("Send mail to: " + email);

        }

        private async Task SendmailAsync(MimeMessage message)
        {
            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);
        }
    }
}
