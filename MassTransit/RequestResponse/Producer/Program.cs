
using MassTransit;
using Shared.RequestReponseMessages;

Console.WriteLine("Publisher started");

string rabbitmqUri = "amqp://localhost:5672/";

string queueName = "masstransit-request-response";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(configure =>
{
    configure.Host(rabbitmqUri);
});
await bus.StartAsync();

var requestClient = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitmqUri}/{queueName}"));

foreach(int x in Enumerable.Range(0, 100))
{
    Console.WriteLine(x);
    
    await Task.Delay(100);

    var response = await requestClient.GetResponse<ResponseMessage>(new RequestMessage
    {
        MessageNo = x,
        Text = $"Message  : {x}"
    });

    Console.WriteLine(response.Message.Text);
}

Console.ReadLine();