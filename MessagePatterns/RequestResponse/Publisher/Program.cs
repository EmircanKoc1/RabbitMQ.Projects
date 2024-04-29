using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.Uri = new Uri("amqp:localhost:5672");
            factory.ClientProvidedName = "Emircanpc";

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();


            string requestQueueName = "example-request-response-queue";

            channel.QueueDeclare(
                queue: requestQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false);

            string replyQueueName = channel.QueueDeclare().QueueName;
            string correlationId = Guid.NewGuid().ToString();

            IBasicProperties properties = channel.CreateBasicProperties();

            properties.CorrelationId = correlationId;
            properties.ReplyTo = replyQueueName;


            Enumerable.Range(0, 100).ToList().ForEach(x =>
            {
                var message = Encoding.UTF8.GetBytes("Merhaba " + x);
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: replyQueueName,
                    basicProperties: properties,
                    body: message);

            });

            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(
                queue: replyQueueName,
                autoAck: true,
                consumer: consumer);

            consumer.Received += (sender, e) =>
            {
                if (e.BasicProperties.CorrelationId == correlationId)
                {
                    Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
                }

            };


        }
    }
}
