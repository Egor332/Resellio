namespace ResellioBackend.Paging
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalAmount {  get; set; }
    }
}
