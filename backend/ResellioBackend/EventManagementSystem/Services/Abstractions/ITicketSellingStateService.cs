using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface ITicketSellingStateService
    {
        public Task<ResultBase> PutTicketOnSaleAsync(SellTicketDto sellingData, int userId);
        public Task<ResultBase> StopSellingTicket(StopSellingTicketDto ticketData, int userId);
    }
}
