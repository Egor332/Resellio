namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class EventInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
