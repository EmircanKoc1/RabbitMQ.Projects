

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

            string exchangeName = "topic-exchange-example";

            channel.ExchangeDeclare(
                 exchange: exchangeName,
                 type: ExchangeType.Topic);


            Console.WriteLine("Lütfen topic1 ismini giriniz");
            var topicName1 = Console.ReadLine();
            Console.WriteLine("Lütfen topic2 ismini giriniz");
            var topicName2 = Console.ReadLine();
            Console.WriteLine("Lütfen topic3 ismini giriniz");
            var topicName3 = Console.ReadLine();
            Console.WriteLine("Lütfen topic4 ismini giriniz");
            var topicName4 = Console.ReadLine();
            Console.WriteLine("Lütfen topic5 ismini giriniz");
            var topicName5 = Console.ReadLine();

            var queueName1 = "queue1";
            var queueName2 = "queue2";
            var queueName3 = "queue3";
            var queueName4 = "queue4";
            var queueName5 = "queue5";

            //Declaring

            channel.QueueDeclare(
                queue: queueName1,
                durable: true,
                exclusive: false);

            channel.QueueDeclare(
                queue: queueName2,
                durable: true,
                exclusive: false);

            channel.QueueDeclare(
                queue: queueName3,
                durable: true,
                exclusive: false);

            channel.QueueDeclare(
                queue: queueName4,
                durable: true,
                exclusive: false);

            channel.QueueDeclare(
                queue: queueName5,
                durable: true,
                exclusive: false);

            //Binding

            channel.QueueBind(
                queue: queueName1,
                exchange: exchangeName,
                routingKey: topicName1);

            channel.QueueBind(
               queue: queueName2,
               exchange: exchangeName,
               routingKey: topicName2);

            channel.QueueBind(
               queue: queueName3,
               exchange: exchangeName,
               routingKey: topicName3);

            channel.QueueBind(
               queue: queueName4,
               exchange: exchangeName,
               routingKey: topicName4);

            channel.QueueBind(
               queue: queueName5,
               exchange: exchangeName,
               routingKey: topicName5);

            EventingBasicConsumer consumer = new(channel);

            consumer.Received += (sender, e) =>
            {
                Console.WriteLine(e.ConsumerTag);
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            };


            channel.BasicConsume(
                queue: queueName1,
                autoAck: true,
                consumer: consumer);

            channel.BasicConsume(
               queue: queueName2,
               autoAck: true,
               consumer: consumer);

            channel.BasicConsume(
               queue: queueName3,
               autoAck: true,
               consumer: consumer);

            channel.BasicConsume(
               queue: queueName4,
               autoAck: true,
               consumer: consumer);

            channel.BasicConsume(
               queue: queueName5,
               autoAck: true,
               consumer: consumer);



            Console.Read();
        }
    }
}
