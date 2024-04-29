using RabbitMQ.Client;
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


            string queueName = "example-work-queue";

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false);

            for (int i = 0; i < 30; i++)
            {

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: queueName,
                    body: Encoding.UTF8.GetBytes($"Message : {i}"));
            }



        }
    }
}
