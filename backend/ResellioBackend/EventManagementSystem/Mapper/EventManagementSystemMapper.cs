using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;

namespace ResellioBackend.EventManagementSystem.Mapper
{
    public static class EventManagementSystemMapper
    {
        public static EventInfoDto EventToEventInfoDto(Event model)
        {
            return new EventInfoDto()
            {
                Id = model.EventId,
                Name = model.Name,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
            };
        }

        public static TicketTypeInfoDto TicketTypeToTicketTypeInfoDto(TicketType model)
        {
            return new TicketTypeInfoDto()
            {
                TypeId = model.TypeId,
                Description = model.Description,
                AvailableFrom = model.AvailableFrom,
                AmountOfTickets = model.MaxCount,
                BasePrice = model.BasePrice,
            };
        }
    }
}
