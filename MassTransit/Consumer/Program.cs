
using Consumer.Consumers;
using MassTransit;


Console.WriteLine("Consumer started");

string rabbitmqUri = "amqp://localhost:5672/";

string queueName = "masstransit";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host(rabbitmqUri, configure =>
    {
        configure.Username("guest");
        configure.Password("guest");
    });

    config.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });

});


await bus.StartAsync();

Console.ReadLine();