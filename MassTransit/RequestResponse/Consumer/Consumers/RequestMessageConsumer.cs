using MassTransit;
using Shared.RequestReponseMessages;

namespace Consumer.Consumers
{
    public class RequestMessageConsumer : IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            var message = context.Message;
            
            Console.WriteLine(message.MessageNo + "Consume edildi");
            
            await context.RespondAsync<ResponseMessage>(new ResponseMessage
            {
                Text = $"{message.MessageNo} Response mesajı "
            });


        }
    }
}
