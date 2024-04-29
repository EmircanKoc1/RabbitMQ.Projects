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


            string exchangeName = "example-pubsub-exchange";

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false);


            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: string.Empty,
                body : Encoding.UTF8.GetBytes("Merhaba"));


            Console.Read();
        }
    }
}
