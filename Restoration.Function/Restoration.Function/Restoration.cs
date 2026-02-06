using System.Text.Json;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Restoration.Function.Data.Repositories;
using Restoration.Function.Models.Enums;
using Restoration.Function.Models.Etities;
using Restoration.Function.ServicesAbstractions;

namespace Restoration.Function;

public class Restoration
{
    private readonly ILogger<Restoration> _logger;
    private readonly IBoardArchiveRepository _boardArchiveRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IArchivationLogsService _archivationLogsService;
    private readonly IMapper _mapper;

    public Restoration(
        ILogger<Restoration> logger, 
        IBoardArchiveRepository boardArchiveRepository, 
        IBlobStorageService blobStorageService, 
        IArchivationLogsService archivationLogsService, 
        IMapper mapper)
    {
        _logger = logger;
        _boardArchiveRepository = boardArchiveRepository;
        _blobStorageService = blobStorageService;
        _archivationLogsService = archivationLogsService;
        _mapper = mapper;
    }

    [Function(nameof(Restoration))]
    public async Task Run(
        [ServiceBusTrigger(RestorationConstants.RestorationQueueName, Connection = "AzureServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            var boardId = JsonSerializer.Deserialize<int>(message.Body.ToString());

            var boardData = await _blobStorageService.DownloadJsonAsync(boardId);
            if (boardData != null)
            {
                await _archivationLogsService.SaveArchivationLogAsync(
                    boardId, 
                    ArchiveStatus.GotDataFromBlobStorage);
                
                var board = _mapper.Map<Board>(boardData);
                
                await _boardArchiveRepository.RestoreBoardData(board);
                
                await _boardArchiveRepository.MarkBoardAsActive(board.Id);
                
                await _blobStorageService.DeleteBlobAsync(board.Id);
                
                await _archivationLogsService.SaveArchivationLogAsync(
                    board.Id, 
                    ArchiveStatus.Restored);
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Failed to deserialize message {message.MessageId}");
            await messageActions.DeadLetterMessageAsync(
                message,
                null,
                "InvalidMessage",
                "Failed to deserialize message");
        }
        catch(Exception ex)
        {
            _logger.LogError($"Failed restore board {message.Body}.");
            await messageActions.DeadLetterMessageAsync(
                message,
                null,
                "Restoration error",
                ex.ToString());
        }
    }
}