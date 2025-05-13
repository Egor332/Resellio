namespace ResellioBackend.Results
{
    public class ResultBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }
}
