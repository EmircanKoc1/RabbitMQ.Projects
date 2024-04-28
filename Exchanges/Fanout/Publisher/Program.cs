using RabbitMQ.Client;
using System.Text;

namespace Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            ConnectionFactory factory = new ConnectionFactory();

            factory.Uri = new Uri("amqp:localhost:5672");
            factory.ClientProvidedName = "Emircanpc";

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            string exchangeName = "fanout-exchange-example";

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Fanout,
                durable: true);

            Enumerable.Range(0, 100).ToList().ForEach(x =>
            {
                Task.Delay(100).Wait();
                var message = "mesaj : " + x;
                Console.WriteLine("yollandı : " + message);
                var byteMessage = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: string.Empty,
                    body: byteMessage);
            });

            Console.Read();

        }
    }
}
