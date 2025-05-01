namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class EventInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string ImageUri { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/8/83/TrumpPortrait.jpg";
    }
}
