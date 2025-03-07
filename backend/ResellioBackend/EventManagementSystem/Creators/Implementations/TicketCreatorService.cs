using ResellioBackend.EventManagementSystem.Creators.Abstractions;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Results;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Creators.Implementations;

public class TicketCreatorService: ITicketCreatorService
{
    public readonly ITicketsRepository _ticketsRepository;

    public TicketCreatorService(ITicketsRepository ticketsRepository)
    {
        _ticketsRepository = ticketsRepository;
    }

    public async Task<Result<Ticket>> CreateTicketAsync(TicketType ticketType)
    {
        Ticket newTicket = new Ticket()
        {
            TicketType = ticketType
        };
        
        await _ticketsRepository.AddAsync(newTicket);

        return new Result<Ticket>()
        {
            Success = true,
            Message = "Created successfully",
            Data = newTicket
        };
    }
}