using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;
using ResellioBackend.TicketPurchaseSystem.Services.Abstractions;
using ResellioBackend.TransactionManager;

namespace ResellioBackend.TicketPurchaseSystem.Services.Implementations
{
    public class TicketSellerService : ITicketSellerService
    {
        private readonly IDatabaseTransactionManager _transactionManager;

        public TicketSellerService(IDatabaseTransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public async Task<ResultBase> TryMarkTicketsAsSoledAsync(List<Ticket> tickets)
        {
            // using var transaction = await _transactionManager.BeginTransactionAsync();           

            throw new NotImplementedException();
        }
    }
}
