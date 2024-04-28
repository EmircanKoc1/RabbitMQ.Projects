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

            string exchangeName = "header-exchange-example";


            channel.ExchangeDeclare(
            exchange: exchangeName,
            type: ExchangeType.Headers,
            durable: true,
            autoDelete: false);

            Enumerable
                .Range(0, 100)
                .ToList()
                .ForEach(x =>
                {
                    string? value = Console.ReadLine();
                    Console.WriteLine(value);
                    var byteMessage = Encoding.UTF8.GetBytes("mesaj");


                    IBasicProperties prop = channel.CreateBasicProperties();

                    prop.Headers = new Dictionary<string, object>
                    {

                        ["no"] = value
                    };


                    channel.BasicPublish(
                        exchange: exchangeName,
                        routingKey: string.Empty,
                        body: byteMessage,
                        basicProperties: prop);

                });



        }

    }
}
