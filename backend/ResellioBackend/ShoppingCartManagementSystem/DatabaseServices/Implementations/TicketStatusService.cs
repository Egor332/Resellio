using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Abstractions;
using ResellioBackend.EventManagementSystem.Enums;
using System.Transactions;

namespace ResellioBackend.ShoppingCartManagementSystem.DatabaseServices.Implementations
{
    public class TicketStatusService : ITicketStatusService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ResellioDbContext _context;

        public TicketStatusService(ITicketsRepository ticketsRepository, ResellioDbContext context)
        {
            _ticketsRepository = ticketsRepository;
            _context = context;
        }

        public async Task<ResultBase> LockTicketInDbAsync(Guid ticketId, DateTime newLockTime)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLock(ticketId);
                if (ticket == null)
                {
                    return new ResultBase
                    {
                        Success = false,
                        Message = "This ticket does not exist"
                    };
                }

                if ((ticket.LastLock != null) && (ticket.LastLock > DateTime.UtcNow))
                {
                    return new ResultBase
                    {
                        Success = false,
                        Message = "This ticket have already been reserved"
                    };
                }

                ticket.TicketState = TicketStates.Reserved;
                ticket.LastLock = newLockTime;
                await _ticketsRepository.UpdateAsync(ticket);

                await transaction.CommitAsync();
                return new ResultBase
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultBase
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResultBase> UnlockTicketInDbAsync(Guid ticketId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLock(ticketId);
                if (ticket == null)
                {
                    return new ResultBase
                    {
                        Success = false,
                        Message = "This ticket does not exist"
                    };
                }

                ticket.TicketState = TicketStates.Available;
                ticket.LastLock = null;
                await _ticketsRepository.UpdateAsync(ticket);

                await transaction.CommitAsync();
                return new ResultBase
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultBase
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
