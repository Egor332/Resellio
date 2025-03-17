using Confluent.Kafka;
using System.Text.Json;

namespace ResellioBackend.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _topic = _configuration["Kafka:Topic"];

            var config = new ProducerConfig()
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessageAsync<T>(T message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            var kafkaMessage = new Message<Null, string>() { Value = jsonMessage };

            await _producer.ProduceAsync(_topic, kafkaMessage);
        }
    }
}
