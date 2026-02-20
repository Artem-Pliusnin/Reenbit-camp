using Application.Subscriptions.Commands.CancelExpiredSubscriptions;
using MediatR;

namespace Infrastructure.BackgroundJobs;

public class CancelExpiredSubscriptionsJob
{
    protected readonly ISender _sender;

    public CancelExpiredSubscriptionsJob(
        ISender Sender)
    {
        _sender = Sender;
    }
    
    public async Task ExecuteAsync()
    {
        var command = new CancelExpiredSubscriptionsCommand();
        
        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            Console.WriteLine("Cancel expired subscriptions job completed.");
        }
        else
        {
            Console.WriteLine($"Cancel expired subscriptions job failed: {result.Error.Message}");
        }
    }
}