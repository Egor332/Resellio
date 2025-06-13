
using NotificationService.Consumers;
using NotificationService.Services.Abstractions;
using NotificationService.Services.Implementations;

namespace NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Services 
            builder.Services.AddTransient<ICustomEmailSender, SendGridEmailSender>();

            builder.Services.AddHealthChecks().AddCheck<KafkaHealthCheck>("kafka");



            builder.Services.AddHostedService<KafkaConsumer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/health/kafka");

            app.Run();
        }
    }
}
