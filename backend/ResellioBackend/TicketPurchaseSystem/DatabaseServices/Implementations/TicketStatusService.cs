using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.EventManagementSystem.Enums;
using System.Transactions;
using ResellioBackend.TransactionManager;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.TicketPurchaseSystem.DatabaseServices.Implementations
{
    public class TicketStatusService : ITicketStatusService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IDatabaseTransactionManager _transactionManager;

        public TicketStatusService(ITicketsRepository ticketsRepository, IDatabaseTransactionManager transactionManager)
        {
            _ticketsRepository = ticketsRepository;
            _transactionManager = transactionManager;
        }

        public async Task<ResultBase> LockTicketInDbAsync(Guid ticketId, DateTime newLockTime)
        {
            using var transaction = await _transactionManager.BeginTransactionAsync();
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

                if ((ticket.LastLock != null && ticket.LastLock > DateTime.UtcNow) || ticket.TicketState == TicketStates.Sold)
                {
                    return new ResultBase
                    {
                        Success = false,
                        Message = "This ticket have already been reserved"
                    };
                }

                ticket.TicketState = TicketStates.Reserved;
                ticket.PurchaseIntender = null;
                ticket.LastLock = newLockTime;
                await _ticketsRepository.UpdateAsync(ticket);

                await _transactionManager.CommitTransactionAsync(transaction);
                return new ResultBase
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync(transaction);
                return new ResultBase
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResultBase> TryMarkAsSoldAsync(Guid ticketId, Customer buyer)
        {
            var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLock(ticketId);
            if (ticket!.TicketState == TicketStates.Sold) 
            {
                return new ResultBase
                {
                    Success = false,
                    Message = "Ticket has already been sold"
                };
            }
            if ((ticket.TicketState == TicketStates.Reserved && ticket.LastLock > DateTime.UtcNow) 
                && ((ticket.PurchaseIntenderId == null) || (ticket.PurchaseIntenderId != buyer.UserId))) 
            {
                return new ResultBase
                {
                    Success = false,
                    Message = "Ticket have already been reserved by somebody else"
                };
            }
            ticket.TicketState = TicketStates.Sold;
            ticket.PurchaseIntender = buyer;
            ticket.Holder = null;
            return new ResultBase
            {
                Success = true,
                Message = $"Ticket: {ticket.TicketId} - sold"
            };
        }

        public async Task<ResultBase> UnlockTicketInDbAsync(Ticket ticket)
        {
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

            return new ResultBase
            {
                Success = true,
            };
        }

        public async Task SetNewLastLockWithRowLockAsync(Guid ticketId, DateTime newLastLock)
        {
            var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLock(ticketId);
            // We assume that ticket is not null here, because ticketId is from stripe or cart
            ticket!.LastLock = newLastLock;
            ticket.TicketState = TicketStates.Reserved;
        }
    }
}
