namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class EventDto
    {
        // no organiser here – we will get it from the JWT token

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<TicketTypeDto> TicketTypeDtos { get; set; }

        public IFormFile EventImage { get; set; }
    }
}