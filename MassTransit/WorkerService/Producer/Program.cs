using Consumer.Services;
using MassTransit;

namespace Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string rabbitmqUri = "amqp://localhost:5672/";

            //string queueName = "masstransit_worker";


            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {


                    services.AddMassTransit(configurator =>
                    {

                        configurator.UsingRabbitMq((_context, _configurator) =>
                        {
                            _configurator.Host(rabbitmqUri);

                        });

                    });
                    services.AddHostedService<PublishMessageService>(provider =>
                    {
                        using IServiceScope scope = provider.CreateScope();
                        IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
                        return new PublishMessageService(publishEndpoint);
                    });

                })
                .Build();

            host.Run();
        }
    }
}