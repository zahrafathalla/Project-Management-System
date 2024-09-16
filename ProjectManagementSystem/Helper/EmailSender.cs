using System.Net.Mail;
using System.Net;

namespace ProjectManagementSystem.Helper
{

    public static class EmailSender
    {
        private static readonly string SenderEmail = "zahragamal546@gmail.com";
        private static readonly string SenderPassword = "mtntiuzbidisthif";

        public static async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(SenderEmail, SenderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
            return true;


        }
    }
}

