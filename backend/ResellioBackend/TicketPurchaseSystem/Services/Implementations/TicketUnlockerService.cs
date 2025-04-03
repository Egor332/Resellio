using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TransactionManager;
using System.Transactions;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class TicketUnlockerService : ITicketUnlockerService
    {
        private readonly ITicketStatusService _ticketStatusService;
        private readonly IRedisService _redisService;
        private readonly IDatabaseTransactionManager _databaseTransactionManager;
        private readonly ITicketsRepository _ticketsRepository;

        public TicketUnlockerService(ITicketStatusService ticketStatusService, IRedisService redisService,
            IDatabaseTransactionManager databaseTransactionManager, ITicketsRepository ticketsRepository)
        {
            _ticketStatusService = ticketStatusService;
            _redisService = redisService;
            _databaseTransactionManager = databaseTransactionManager;
            _ticketsRepository = ticketsRepository;
        }

        public async Task<ResultBase> UnlockTicketAsync(int userId, Guid ticketId)
        {
            await _redisService.DeleteFromCartAsync(ticketId, userId);
            using var transaction = await _databaseTransactionManager.BeginTransactionAsync();
            try
            {
                var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLock(ticketId);
                var unlockInCash = await _redisService.UnlockTicketAsync(ticketId, userId);
                if (!unlockInCash.Success)
                {
                    await _databaseTransactionManager.RollbackTransactionAsync(transaction);
                    return new ResultBase
                    {
                        Success = false,
                        Message = unlockInCash.Message,
                    };
                }
                await _ticketStatusService.UnlockTicketInDbAsync(ticket);
                await _databaseTransactionManager.CommitTransactionAsync(transaction);
                return new ResultBase
                {
                    Success = true,
                    Message = "Ticket unlocked"
                };

            }
            catch (Exception ex)
            {
                await _databaseTransactionManager.RollbackTransactionAsync(transaction);
                return new ResultBase
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
