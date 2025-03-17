namespace NotificationService.Models
{
    public class EmailMessageModel
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}
