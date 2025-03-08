namespace ResellioBackend.Results;

public class GeneralResult<T>: ResultBase
{
    public T Data { get; set; }
}