namespace ResellioBackend.Common.Paging
{
    public interface IPaginationService
    {
        public Task<PaginationResult<T>> ApplyPaginationAsync<T>(IQueryable<T> query, int page, int pageSize);
    }
}
