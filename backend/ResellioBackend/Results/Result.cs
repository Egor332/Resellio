namespace ResellioBackend.Results;

public class Result<T>: ResultBase
{
    public T Data { get; set; }
}