namespace ResellioBackend.Common.Filters
{
    public interface IFiltrable<T>
    {
        public IQueryable<T> ApplyFilters(IQueryable<T> query);
    }
}
