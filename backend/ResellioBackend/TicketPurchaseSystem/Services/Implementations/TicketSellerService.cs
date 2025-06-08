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
        private readonly ITicketUnlockerService _ticketUnlockerService;

        public TicketSellerService(IDatabaseTransactionManager transactionManager, ITicketStatusService ticketStatusService, ITicketUnlockerService ticketUnlockerService)
        {
            _transactionManager = transactionManager;
            _ticketStatusService = ticketStatusService;
            _ticketUnlockerService = ticketUnlockerService;
        }

        public async Task<ResultBase> TryMarkTicketsAsSoldAsync(List<Guid> ticketIds, Customer buyer)
        {
            using var transaction = await _transactionManager.BeginTransactionAsync();
            try
            {
                foreach (var ticketId in ticketIds)
                {
                    var ticketSellingResult = await _ticketStatusService.TryMarkAsSoldAsync(ticketId, buyer);
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

                foreach (var ticketId in ticketIds)
                {
                    await _ticketUnlockerService.UnlockTicketAsync(buyer.UserId, ticketId); // No validation in case of error, but it seems me that it is not necessary here, TODO: if something goes wrong look here
                }

                return new ResultBase()
                {
                    Success = true,
                    Message = "All ticket were assigned to the buyer"
                };
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync(transaction);
                return new ResultBase()
                {
                    Success = false,
                    Message = $"Error: " + ex.Message
                };
            }
        }
    }
}
