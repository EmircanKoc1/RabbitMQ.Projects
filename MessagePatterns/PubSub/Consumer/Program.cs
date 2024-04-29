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


            string exchangeName = "example-pubsub-exchange";

            var q1 = channel.QueueDeclare().QueueName;
            var q2 = channel.QueueDeclare().QueueName;

            channel.ExchangeDeclare(
               exchange: exchangeName,
               type: ExchangeType.Fanout,
               durable: false,
               autoDelete: false);


            channel.QueueBind(
                queue: q1,
                exchange: exchangeName,
                string.Empty);

            channel.QueueBind(
               queue: q2,
               exchange: exchangeName,
               string.Empty);

            EventingBasicConsumer consumer = new(channel);

            consumer.Received += (sender, e) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            };


            channel.BasicConsume(
                queue: q1, consumer: consumer);

            channel.BasicConsume(
               queue: q2, consumer: consumer);

            Console.Read();

        }




    }
}
