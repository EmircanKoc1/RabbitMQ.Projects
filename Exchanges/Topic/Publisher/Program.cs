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

            string exchangeName = "topic-exchange-example";
          

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Topic);

            Enumerable.Range(0, 100).ToList().ForEach(x =>
            {
                Console.WriteLine("Topic giriniz");
                string topic = Console.ReadLine();
                ReadOnlyMemory<byte> message = Encoding.UTF8.GetBytes("message : " + x.ToString());

                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: topic,
                    body: message);

                Task.Delay(100).Wait();

            });


        }


    }
}
