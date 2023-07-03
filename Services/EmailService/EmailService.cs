using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace simpleapi.Services.EmailService
{
    public class EmailService : IEmailService
    {

        // taken from appsettings 
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            // Prepare the body sending Information 
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body};

            // SMTP Information and Connection
            using var smtp = new SmtpClient();
            // host, port, security 
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            // username, password
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);

            // Sending an email
            smtp.Send(email);

            smtp.Disconnect(true);
        }
    }
}
