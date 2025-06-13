using Confluent.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Text.Json;

namespace NotificationService.Services.Implementations
{
    public class KafkaHealthCheck : IHealthCheck
    {
        private readonly string _bootstrapServers;

        public KafkaHealthCheck(IConfiguration configuration)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"];
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

                using var producer = new ProducerBuilder<Null, string>(config).Build();
                var jsonMessage = JsonSerializer.Serialize("message");
                var kafkaMessage = new Message<Null, string>() { Value = jsonMessage };
                producer.Produce("test", kafkaMessage);
                return Task.FromResult(HealthCheckResult.Healthy("Kafka is reachable."));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Kafka is not reachable.", ex));
            }
        }
    }
}
