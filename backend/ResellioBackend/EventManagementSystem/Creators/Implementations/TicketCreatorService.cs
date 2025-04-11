using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations
{

    public class TicketCreatorService : ITicketCreatorService
    {
        public readonly ITicketsRepository _ticketsRepository;

        public TicketCreatorService(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        public GeneralResult<Ticket> CreateTicket(TicketType ticketType)
        {
            Ticket newTicket = new Ticket()
            {
                TicketType = ticketType,
                TicketState = Enums.TicketStates.Available,
                LastLock = null,
                Holder = ticketType.Event.Organiser,
                CurrentPrice = new Money()
                {
                    Amount = ticketType.BasePrice.Amount,
                    CurrencyCode = ticketType.BasePrice.CurrencyCode,
                }
            };

            return new GeneralResult<Ticket>()
            {
                Success = true,
                Message = "Created successfully",
                Data = newTicket
            };
        }
    }
}