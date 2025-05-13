using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResellioBackend.EventManagementSystem.DTOs
{
    public class EventDto
    {
        // no organiser here – we will get it from the JWT token

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string TicketTypeDtos { get; set; }

        [JsonIgnore]
        public List<TicketTypeDto> TicketTypeDtosList => string.IsNullOrEmpty(TicketTypeDtos)
            ? new List<TicketTypeDto>()
            : JsonSerializer.Deserialize<List<TicketTypeDto>>(TicketTypeDtos, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        public IFormFile EventImage { get; set; }
    }
}