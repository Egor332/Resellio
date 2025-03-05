namespace ResellioBackend.Kafka
{
    public interface IKafkaProducerService
    {
        public Task SendMessageAsync<T>(T message);
    }
}
