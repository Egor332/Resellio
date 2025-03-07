namespace ResellioBackend.EventManagementSystem.Results;

public class Result<T>: ResultBase
{
    public T Data { get; set; }
}