
namespace simpleapi.Services.EmailService
{
    public interface IEmailService
    {

        // I take EmailDto from Model/EmailDto 
        void SendEmail(EmailDto request);
     }
}
