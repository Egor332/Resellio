using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions
{

    public interface ITicketTypeCreatorService
    {
        public GeneralResult<TicketType> CreateTicketType(TicketTypeDto ticketTypeDto, Event createdEvent);
    }
}