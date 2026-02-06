namespace Application.Abstractions.Services;

public interface IMessageQueueService
{
    Task SendMessageAsync<T>(T message, string queueName, CancellationToken cancellationToken = default);
}