using System.Text;
using DBABookLibrary.IReadRepository.Service;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DBABookLibrary.IReadRepository.Messaging;

public class InitializeMessaging(
    IConnection connection, 
    IServiceProvider serviceProvider) : BackgroundService
{
    private IBookService BookService => serviceProvider.GetRequiredService<IBookService>();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string queueName = "dba-queue";

        var messageChannel = connection.CreateModel();

        messageChannel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new EventingBasicConsumer(messageChannel);

        consumer.Received += async (_, args) =>
        {
            var body = args.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            await BookService.UpdateBook(message);
        };

        messageChannel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}