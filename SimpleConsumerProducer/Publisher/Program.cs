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


            //durable : kuyruktaki mesajların kalıcılığı 
            //exclusive bu kuyrugun özel olup olmadıgı yani birden fazla bağlantı ile işlem yapılıp yapılamayacağı ile ilgili

            channel.QueueDeclare(
                queue: "example-queue",
                exclusive: false);

            var message = Encoding.UTF8.GetBytes("mesaj");

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "example-queue",
                body: message);

            
            Console.Read();

        }
    }
}
