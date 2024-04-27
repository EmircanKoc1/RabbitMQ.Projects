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

            string queueName = "example-queue";

            channel.QueueDeclare(
                queue: queueName,
                //birden fazla bağlantınının kuyrugua erişmesi için false olmalı 
                exclusive: false,
                //durable parametresi  queue nın kalıcı olmasını sağlıyor , publisher kapansa dahi queue kaybolmaz 
                durable: true);





            EventingBasicConsumer consumer = new(channel);

            var consumerTag = channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            //mesajların dengeli şekilde dağılması için
            channel.BasicQos(
               prefetchSize: 0,
               prefetchCount: 1,
               global: false);




            consumer.Received += (sender, e) =>
            {


                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

                //mesajın işlendiğini belirtir acknowledgement etti
                channel.BasicAck(
                    deliveryTag: e.DeliveryTag,
                    multiple: false);


                //mesajın işlenemediğini belirtir kuyrugua geri gönderiyor
                //channel.BasicNack(
                //    deliveryTag: e.DeliveryTag,
                //    multiple: false,
                //    requeue: true);

            };

            //verilen queue da ki tüm mesajlar reddedilerek işlenmez
            channel.BasicCancel(consumerTag);

            channel.BasicReject(
                deliveryTag: 3,
                requeue: true);

            Console.Read();


        }
    }
}
