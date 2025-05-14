using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Mapper;
using ResellioBackend.EventManagementSystem.Models.Base;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using ResellioBackend.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _ticketRepository;
        private readonly IGetRequestService _getRequestService;

        public TicketService(ITicketsRepository ticketsRepository, IGetRequestService getRequestService)
        {
            _ticketRepository = ticketsRepository;
            _getRequestService = getRequestService;
        }

        public async Task<PaginationResult<TicketInfoDto>> GetMyTicketsAsync(int userId, IFiltrable<Ticket> filter, int page = 1, int pageSize = 10)
        {
            var myTicketsQuery = _ticketRepository.GetMyTicketsAsQueryableNoTracking(userId);
            try
            {
                return await _getRequestService.ApplyFiltersAndPaginationAsync<Ticket, TicketInfoDto>
                    (myTicketsQuery, EventManagementSystemMapper.TicketToTicketInfoDto, filter, page, pageSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        public async Task<PaginationResult<TicketInfoDto>> GetTicketsForSaleOfThisType(int ticketTypeId, IFiltrable<Ticket> filter, int page = 1, int pageSize = 10)
        {         
            var ticketsQuery = _ticketRepository.GetTicketsOfTypeNoTracking(ticketTypeId);
            ticketsQuery = ticketsQuery
                .Where(t => (t.LastLock < DateTime.UtcNow) && 
                            ((t.TicketState == Enums.TicketStates.Available) || (t.TicketState == Enums.TicketStates.Reserved)));
            try
            {
                return await _getRequestService.ApplyFiltersAndPaginationAsync<Ticket, TicketInfoDto>
                    (ticketsQuery, EventManagementSystemMapper.TicketToTicketInfoDto, filter, page, pageSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }
    }
}
