
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

        public KafkaConsumer(ICustomEmailSender emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;

            var config = new ConsumerConfig
            {
                GroupId = "email-service-group",
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe("email-notifications");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);
                        var emailMessage = JsonSerializer.Deserialize<EmailMessageModel>(consumeResult.Message.Value);
                        await _emailSender.SendEmailAsync(emailMessage);
                        Console.WriteLine("Email sent");

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
            _consumer.Close();
            return Task.CompletedTask;
        }
    }
}
