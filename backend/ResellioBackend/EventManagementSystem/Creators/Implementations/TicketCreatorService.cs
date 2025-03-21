using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations;

public class TicketCreatorService: ITicketCreatorService
{
    public readonly ITicketsRepository _ticketsRepository;

    public TicketCreatorService(ITicketsRepository ticketsRepository)
    {
        _ticketsRepository = ticketsRepository;
    }

    public async Task<GeneralResult<Ticket>> CreateTicketAsync(TicketType ticketType)
    {
        Ticket newTicket = new Ticket()
        {
            TicketType = ticketType,
            TicketState = Enums.TicketStates.Available,
            LastLock = null,
        };

        return new GeneralResult<Ticket>()
        {
            Success = true,
            Message = "Created successfully",
            Data = newTicket            
        };
    }
}