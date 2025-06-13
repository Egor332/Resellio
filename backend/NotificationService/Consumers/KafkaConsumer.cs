
using Confluent.Kafka;
using NotificationService.Models;
using NotificationService.Services.Abstractions;
using System.Text.Json;

namespace NotificationService.Consumers
{
    public class KafkaConsumer : IHostedService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ICustomEmailSender _emailSender;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(ICustomEmailSender emailSender, IConfiguration configuration,
            ILogger<KafkaConsumer> logger)
        {
            _emailSender = emailSender;
            _logger = logger;

            var config = new ConsumerConfig
            {
                GroupId = "email-service-group",
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            var topic = configuration["Kafka:Topic"];
            _consumer.Subscribe(topic);

            _logger.LogInformation("Kafka consumer subscribed to topic {Topic}", topic);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka consumer starting...");

            Task.Run(async () =>
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = _consumer.Consume(cancellationToken);
                            _logger.LogInformation("Message received.");
                            var emailMessage = JsonSerializer.Deserialize<EmailMessageModel>(consumeResult.Message.Value);
                            await _emailSender.SendEmailAsync(emailMessage);
                            _logger.LogInformation("Email sent to: {To}", emailMessage.Email);
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogError(ex, "Kafka consume exception");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Unexpected error in Kafka consumer loop");
                        }
                    }
                }
                catch (OperationCanceledException) { }
                finally
                {
                    _consumer.Close();
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kafka consumer stopping...");
            _consumer.Close();
            return Task.CompletedTask;
        }
    }
}
