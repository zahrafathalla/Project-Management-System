using MediatR;
using System.Net.Mail;
using System.Net;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class SendVerificationEmailCommand : IRequest<bool>
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class SendVerificationEmailCommandHendler : IRequestHandler<SendVerificationEmailCommand,bool>
    {
        public async Task<bool> Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
        {
          
            var senderEmail = "zahragamal546@gmail.com";
            var senderPassword = "mtntiuzbidisthif";


            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword) ,
                EnableSsl = true

            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = true 
            };
            mailMessage.To.Add(request.ToEmail);

            await client.SendMailAsync(mailMessage);

            return true;

        }
    }

}
