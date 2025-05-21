using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface ITicketService
    {
        public Task<PaginationResult<TicketInfoDto>> GetMyTicketsAsync(int userId, IFiltrable<Ticket> filter, int page = 1, int pageSize = 10);
        public Task<PaginationResult<TicketInfoDto>> GetTicketsForSaleOfThisType(int ticketTypeId, IFiltrable<Ticket> filter, int page = 1, int pageSize = 10);
    }
}
