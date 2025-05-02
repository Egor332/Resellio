using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Filtering;
using ResellioBackend.EventManagementSystem.Models;
using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class GetRequestService : IGetRequestService
    {
        private readonly IPaginationService _pagingService;

        public GetRequestService(IPaginationService pagingService)
        {
            _pagingService = pagingService;
        }

        public async Task<PaginationResult<TDto>> ApplyFiltersAndPaginationAsync<TModel, TDto>
            (IQueryable<TModel> query, Func<TModel, TDto> mapper, IFiltrable<TModel> filter, int page = 1, int pageSize = 10)
        {
            query = filter.ApplyFilters(query);
            var paginatedModels = new PaginationResult<TModel>();
            try
            {
                paginatedModels = await _pagingService.ApplyPaginationAsync<TModel>(query, page, pageSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }

            var paginationResponse = new PaginationResult<TDto>()
            {
                Items = new List<TDto>(),
                PageNumber = paginatedModels.PageNumber,
                TotalAmount = paginatedModels.TotalAmount,
            };
            foreach (var item in paginatedModels.Items) 
            { 
                var itemDto = mapper(item);
                paginationResponse.Items.Add(itemDto);
            }

            return paginationResponse;
        }
    }
}
