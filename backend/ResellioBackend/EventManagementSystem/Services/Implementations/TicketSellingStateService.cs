using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.TransactionManager;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Statics;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class TicketSellingStateService : ITicketSellingStateService
    {
        private readonly IUsersRepository<UserBase> _userRepository;
        private readonly ITicketsRepository _ticketRepository;
        private readonly IDatabaseTransactionManager _transactionManager;

        public TicketSellingStateService(IUsersRepository<UserBase> userRepository, ITicketsRepository ticketRepository,
            IDatabaseTransactionManager transactionManager)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _transactionManager = transactionManager;
        }

        public async Task<ResultBase> ResellTicketAsync(ResellDto sellingData, int userId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(sellingData.TicketId);
            var user = await _userRepository.GetByIdAsync(userId);
            if ((ticket == null) || (ticket.HolderId != userId) || (user == null)) 
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Ticket was not found in users tickets"
                };
            }
            if (!user.ValidateAbilityToSale())
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "You did not connect seller account",
                    ErrorCode = UserManagementSystemErrorsCodes.UserDoesNotConnectSellerAccount
                };
            }

            if (!ticket.PutTicketOnSale(sellingData.Price, sellingData.Currency))
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Unavailable currency provided" // kind of strange, but no other errors  could happen there, so I think ResultBase will be overkill
                };
            }

            await _ticketRepository.UpdateAsync(ticket);

            return new ResultBase()
            {
                Success = true,
            };

        }

        public async Task<ResultBase> StopSellingTicket(StopSellingTicketDto ticketData, int userId)
        {
            using var transaction = await _transactionManager.BeginTransactionAsync();

            var ticket = await _ticketRepository.GetTicketByIdWithExclusiveRowLockAsync(ticketData.TicketId);
            var user = await _userRepository.GetByIdAsync(userId);
            if ((ticket == null) || (ticket.HolderId != userId) || (user == null))
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Ticket was not found in users tickets"
                };
            }

            ticket.StopSellingTicket();
            await _ticketRepository.UpdateAsync(ticket);

            await _transactionManager.CommitTransactionAsync(transaction);


            return new ResultBase()
            {
                Success = true,
            };
        }

        
    }
}
