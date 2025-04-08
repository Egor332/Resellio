using ResellioBackend.EventManagementSystem.Enums;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisRepositories.Abstractions;
using ResellioBackend.TicketPurchaseSystem.RedisServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TransactionManager;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class PurchaseLockService : IPurchaseLockService
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IRedisService _redisService;
        private readonly IDatabaseTransactionManager _transactionManager;
        private readonly ICartRedisRepository _cartRedisRepository;

        public PurchaseLockService(ITicketsRepository ticketsRepository, IRedisService redisService, 
            IDatabaseTransactionManager transactionManager, ICartRedisRepository cartRedisRepository)
        { 
            _ticketsRepository = ticketsRepository;
            _redisService = redisService;
            _transactionManager = transactionManager;
            _cartRedisRepository = cartRedisRepository;
        }

        public async Task<ResultBase> EnsureEnoughLockTimeForPurchaseAsync(int userId)
        {
            var cartLifeTime = await _cartRedisRepository.GetExpirationTimeAsync(userId);
            if (cartLifeTime == null)
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Lock have already expired"
                };
            }
            var maximumLockExtension = DateTime.UtcNow.AddMinutes(5);
            var currentLockExpiration = DateTime.UtcNow + cartLifeTime;
            if (maximumLockExtension < currentLockExpiration)
            {
                return new ResultBase()
                {
                    Success = true
                };
            }
            var ticketIds = await _cartRedisRepository.GetAllTicketsAsync(userId);
            var isTicketLockSucceed = await TryChangeLockForTicketEnumerationAsync(ticketIds, userId, maximumLockExtension);
            if (!isTicketLockSucceed) 
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Error occurred, possibly your shopping cart expired"
                };
            }

            await ChangeLockInDatabaseAsync(ticketIds, userId, maximumLockExtension, TicketStates.Reserved);

            return new ResultBase()
            {
                Success = true,
            };
        }

        public async Task GetAddedTimeBackAsync(List<Guid> ticketIds, int userId)
        {
            var cartLifeTime = await _cartRedisRepository.GetExpirationTimeAsync(userId);
            var realExpirationTime = DateTime.UtcNow;
            if (cartLifeTime == null)
            {
                await RemoveAllTicketLocksAsync(ticketIds, userId);
            }
            else
            {
                realExpirationTime += cartLifeTime.Value;
                await TryChangeLockForTicketEnumerationAsync(ticketIds, userId, realExpirationTime);
            }            
            
            using var transaction = await _transactionManager.BeginTransactionAsync();
            foreach (var ticketId in ticketIds)
            {
                var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLockAsync(ticketId);
                if ((ticket!.PurchaseIntenderId == null) || (ticket.PurchaseIntenderId != userId))
                {
                    continue;
                }
                ticket.ChangeLockParameters(null, TicketStates.Available, null);
            }
            await _transactionManager.CommitTransactionAsync(transaction);
        }

        private async Task RemoveAllTicketLocksAsync(List<Guid> ticketIds, int userId)
        {
            foreach (var ticketId in ticketIds)
            {
                await _redisService.UnlockTicketAsync(ticketId, userId);
            }
        }

        private async Task<bool> TryChangeLockForTicketEnumerationAsync(IEnumerable<Guid> ticketIds, int userId, DateTime targetExpirationTime)
        {
            var ticketWithSuccessfulExtension = new List<Guid>();
            foreach (var ticketId in ticketIds)
            {
                var lockExtension = targetExpirationTime - DateTime.UtcNow;
                var ticketLockResult = await _redisService.ChangeExpirationTimeForTicketAsync(ticketId, lockExtension, userId);
                if (ticketLockResult)
                {
                    ticketWithSuccessfulExtension.Add(ticketId);
                }
                else
                {
                    await RemoveAllTicketLocksAsync(ticketWithSuccessfulExtension, userId);
                    return false;
                }
            }
            return true;
        }
        
        private async Task ChangeLockInDatabaseAsync(IEnumerable<Guid> ticketIds, int? userId, DateTime? newLockTime, TicketStates newStatus) 
        {
            using var transaction = await _transactionManager.BeginTransactionAsync();
            foreach(var ticketId in ticketIds)
            {
                var ticket = await _ticketsRepository.GetTicketByIdWithExclusiveRowLockAsync(ticketId);
                ticket!.ChangeLockParameters(newLockTime, newStatus, userId);
            }
            await _transactionManager.CommitTransactionAsync(transaction);
        }
    }
}
