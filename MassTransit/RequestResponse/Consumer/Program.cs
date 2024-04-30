
using Consumer.Consumers;
using MassTransit;

Console.WriteLine("Consumer started");

string rabbitmqUri = "amqp://localhost:5672/";

string queueName = "masstransit-request-response";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(configure =>
{
    configure.Host(rabbitmqUri);

    configure.ReceiveEndpoint(queueName, config =>
    {
        config.Consumer<RequestMessageConsumer>();
    });

});

await bus.StartAsync();

Console.ReadLine();