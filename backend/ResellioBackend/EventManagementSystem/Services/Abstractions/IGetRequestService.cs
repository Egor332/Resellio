using ResellioBackend.Common.Filters;
using ResellioBackend.Common.Paging;
using ResellioBackend.EventManagementSystem.DTOs;
using ResellioBackend.EventManagementSystem.Filtering;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IGetRequestService
    {
        public Task<PaginationResult<TDto>> ApplyFiltersAndPaginationAsync<TModel, TDto>
            (IQueryable<TModel> query, Func<TModel, TDto> mapper, IFiltrable<TModel> filter, int page = 1, int pageSize = 10);
    }
}
