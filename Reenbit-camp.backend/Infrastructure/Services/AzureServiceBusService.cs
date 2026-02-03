using System.Text.Json;
using Application.Abstractions.Services;
using Azure.Messaging.ServiceBus;

namespace Infrastructure.Services;

public class AzureServiceBusService : IMessageQueueService
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusService(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task SendMessageAsync<T>(
        T message, 
        string queueName, 
        CancellationToken cancellationToken = default)
    {
        await using var sender = _client.CreateSender(queueName);
            
        var jsonMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(jsonMessage)
        {
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString()
        };

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}