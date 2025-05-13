using System.Text;
using RabbitMQ.Client;

namespace DBABookLibrary.WriteRepository.Messaging;

public sealed class Messaging(IConnection connection) : IMessaging
{
    public void Sender(string guid)
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "dba-queue",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        var body = Encoding.UTF8.GetBytes(guid);

        channel.BasicPublish(exchange: string.Empty, routingKey: "dba-queue", body: body);
    }
}