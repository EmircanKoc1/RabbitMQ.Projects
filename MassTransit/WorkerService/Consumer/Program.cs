using Consumer.Consumers;
using MassTransit;

namespace Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {


            string rabbitmqUri = "amqp://localhost:5672/";

            string queueName = "masstransit_worker";

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddMassTransit(configurator =>
                    {
                        configurator.AddConsumer<ExampleMessageConsumer>();

                        configurator.UsingRabbitMq((_context, _configurator) =>
                        {
                            _configurator.Host(rabbitmqUri);

                            _configurator.ReceiveEndpoint(queueName, e =>
                            {
                                e.ConfigureConsumer<ExampleMessageConsumer>(_context);
                            });
                        });

                    });



                })
                .Build();



            host.Run();
        }
    }
}