namespace ResellioBackend.UserManagementSystem.Models.Emails
{
    public class VerificationEmail
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}
