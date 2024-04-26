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
            factory.ClientProvidedName = "Emircanpc2";

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();


            channel.QueueDeclare(
                queue: "example-queue",
                exclusive: false);

            //EventingBasicConsumer consumer = new(channel);

            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

            //};


            //channel.BasicConsume(
            //    queue: "example-queue",
            //    autoAck: false,
            //    consumer: consumer);


            channel.QueueConsume(
            queueName: "example-queue",
            action: (message) =>
            {
                Console.WriteLine(message);
            });


            Console.Read();

        }


    }
    public static class Exto
    {

        public static void QueueConsume(this IModel channel, string queueName, Action<string> action)
        {
            EventingBasicConsumer consumer = new(channel);

            consumer.Received += (sender, e) =>
            {
                action(e.Body.Span.ByteSpanToString());
            };


            channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer);

        }

        private static string ByteSpanToString(this ReadOnlySpan<byte> span)
            => Encoding.UTF8.GetString(span);


    }


}
