using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer
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

            EventingBasicConsumer consumer = new(channel);

            channel.BasicConsume(
                queue: requestQueueName,
                autoAck: true,
                consumer: consumer);


            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);

                Console.WriteLine(message);

                var responseMessage = Encoding.UTF8.GetBytes("İşlem tamamlandı" + message);

                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = e.BasicProperties.CorrelationId;

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: e.BasicProperties.ReplyTo,
                    basicProperties: properties,
                    body: responseMessage);

            };



        }
    }
}
