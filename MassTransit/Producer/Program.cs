using MassTransit;
using Shared.Messages;


Console.WriteLine("Producer started");

string rabbitmqUri = "amqp://localhost:5672/";
string queueName = "masstransit";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host(rabbitmqUri,configure =>
    {
        configure.Username("guest");
        configure.Password("guest");
    });
});


ISendEndpoint sendEndpoint =  await bus.GetSendEndpoint(new Uri($"{rabbitmqUri}/{queueName}"));

Console.Write("Gönderilecek mesaj");
string? message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new Message
{
    Text = message
});


Console.ReadLine();