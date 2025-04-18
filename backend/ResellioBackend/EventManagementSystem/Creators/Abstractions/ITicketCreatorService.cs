using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Creators.Abstractions
{
    public interface ITicketCreatorService
    {
        public GeneralResult<Ticket> CreateTicket(TicketType ticketType);
    }
}