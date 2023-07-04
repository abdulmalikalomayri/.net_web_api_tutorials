namespace simpleapi.Models
{
    public class EmailDto
    {
        // This model the attributes for SMTP 
        // I will put model in the IEmailService
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = "<h1>test from model</h1>";
    }
}
