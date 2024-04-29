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


            string queueName = "example-work-queue";

            channel.QueueDeclare(
               queue: queueName,
               durable: false,
               exclusive: false,
               autoDelete: false);

            EventingBasicConsumer consumer = new(channel);

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            channel.BasicQos(
                prefetchSize: 1,
                prefetchCount: 0,
                global: false);

            consumer.Received += (sender, e) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            };

        }


    }
}
