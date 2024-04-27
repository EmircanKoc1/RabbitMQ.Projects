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

            string routingKey = "direct-queue-example";
            string exchangeName = "direct-exchange-example";

           // var message = Encoding.UTF8.GetBytes("Merhaba");

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct);

            while (true)
            {
                string message = Console.ReadLine();
                var byteMessage = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    body: byteMessage);
            }

            Console.Read();


        }
    }
}
