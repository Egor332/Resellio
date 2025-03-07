namespace ResellioBackend.EventManagementSystem.Results;

public class Result<T>: ResultsBase
{
    public T Data { get; set; }
}