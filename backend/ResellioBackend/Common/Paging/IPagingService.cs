namespace ResellioBackend.Common.Paging
{
    public interface IPagingService
    {
        public Task<PagingResult<T>> ApplyPagingAsync<T>(IQueryable<T> query, int page, int pageSize);
    }
}
