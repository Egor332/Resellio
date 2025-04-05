using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.DatabaseServices.Abstractions;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TransactionManager;
using ResellioBackend.UserManagementSystem.Models.Users;
using System.Runtime.CompilerServices;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class TicketSellerService : ITicketSellerService
    {
        private readonly IDatabaseTransactionManager _transactionManager;
        private readonly ITicketStatusService _ticketStatusService;

        public TicketSellerService(IDatabaseTransactionManager transactionManager, ITicketStatusService ticketStatusService)
        {
            _transactionManager = transactionManager;
            _ticketStatusService = ticketStatusService;
        }

        public async Task<ResultBase> TryMarkTicketsAsSoledAsync(List<Guid> ticketIds, Customer buyer)
        {
            using var transaction = await _transactionManager.BeginTransactionAsync();
            foreach (var ticketId in ticketIds)
            {
                var ticketSellingResult = await _ticketStatusService.TryMarkAsSoledAsync(ticketId, buyer);
                if (!ticketSellingResult.Success) 
                {
                    await _transactionManager.RollbackTransactionAsync(transaction);
                    return new ResultBase()
                    {
                        Success = false,
                        Message = $"Error buying ticket: {ticketId} : " + ticketSellingResult.Message
                    };
                }

            }

            await _transactionManager.CommitTransactionAsync(transaction);
            return new ResultBase()
            {
                Success = true,
                Message = "All ticket were assigned to the buyer"
            };
        }
    }
}
