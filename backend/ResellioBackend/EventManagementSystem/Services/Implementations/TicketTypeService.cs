using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Mapper;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Repositories.Implementations;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypesRepository _ticketTypesRepository;
        private readonly IGetRequestService _getRequestService;

        public TicketTypeService(ITicketTypesRepository ticketTypesRepository, IGetRequestService getRequestService)
        {
            _ticketTypesRepository = ticketTypesRepository;
            _getRequestService = getRequestService;
        }

        public async Task<PaginationResult<TicketTypeInfoDto>> GetTicketTypesOfEventAsync(IFiltrable<TicketType> filter, int page = 1, int pageSize = 10)
        {
            var query = _ticketTypesRepository.GetAllAsQueryable();
            try
            {
                return await _getRequestService.ApplyFiltersAndPaginationAsync<TicketType, TicketTypeInfoDto>
                    (query, EventManagementSystemMapper.TicketTypeToTicketTypeInfoDto, filter, page, pageSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }
    }
}
