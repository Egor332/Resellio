using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;

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
                OrganiserId = model.OrganiserId,
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
                EventId = model.EventId,
            };
        }

        public static TicketInfoDto TicketToTicketInfoDto(Ticket model)
        {
            return new TicketInfoDto()
            {
                Id = model.TicketId,
                IsOnSale = (model.TicketState == Enums.TicketStates.Available) || (model.TicketState == Enums.TicketStates.Reserved),
                CurrentPrice = model.GetPrice(),
                EventName = model.TicketType.Event.Name,
                EventDescription = model.TicketType.Event.Description,
                TicketTypeDescription = model.TicketType.Description,
                IsHoldByOrganiser = (model.HolderId == model.TicketType.Event.OrganiserId),
                EventId = model.TicketType.TypeId,
                TicketTypeId = model.TicketTypeId,
            };
        }
    }
}
