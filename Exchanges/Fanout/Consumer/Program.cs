using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer
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
            var queueName = "queue1";


            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Fanout,
                durable: true);

            channel.QueueDeclare(
                queue: queueName,
                exclusive: false);

            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: string.Empty);

            EventingBasicConsumer consumer = new(channel);

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            consumer.Received += (sender, e) =>
            {

                string message = Encoding.UTF8.GetString(e.Body.Span);

                Console.WriteLine("alındı : " + message);
            };


            Console.ReadLine();
        }
    }
}
