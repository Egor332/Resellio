namespace ResellioBackend.Common.Paging
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalAmount {  get; set; }
    }
}
