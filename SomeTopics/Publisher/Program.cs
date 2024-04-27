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

            string queueName = "example-queue";

            channel.QueueDeclare(
                queue: queueName,
                exclusive: false,
                durable: true);

            //publish de kalıcılık sağlamk için
            IBasicProperties prop = channel.CreateBasicProperties();
            prop.Persistent = true;

           


            Enumerable.Range(0, 100).ToList().ForEach(x =>
            {
                Task.Delay(100).Wait();
                var message = Encoding.UTF8.GetBytes("merhaba" + x);

                channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                body: message,
                basicProperties: prop);

            });


            Console.Read();

        }
    }
}
