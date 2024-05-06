using MassTransit;
using Shared.Messages;

namespace Consumer.Services
{
    internal class PublishMessageService : BackgroundService
    {

        private readonly IPublishEndpoint _publishEndpoint;

        public PublishMessageService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (true)
            {
                Message message = new Message
                {
                    Text = "message  : " + i++
                };

                await _publishEndpoint.Publish(message);
            }

        }
    }
}
