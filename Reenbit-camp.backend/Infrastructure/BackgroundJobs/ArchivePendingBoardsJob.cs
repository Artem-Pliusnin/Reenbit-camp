using Application.Boards.Commands.ArchivePendingBoards;
using MediatR;

namespace Infrastructure.BackgroundJobs;

public class ArchivePendingBoardsJob
{
    protected readonly ISender _sender;

    public ArchivePendingBoardsJob(
        ISender Sender)
    {
        _sender = Sender;
    }

    public async Task ExecuteAsync()
    {
        var command = new ArchivePendingBoardsCommand();
        
        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            Console.WriteLine($"Archive pending boards job completed. Archived {result.Value} boards");
        }
        else
        {
            Console.WriteLine($"Archive pending boards job failed: {result.Error.Message}");
        }
    }
}