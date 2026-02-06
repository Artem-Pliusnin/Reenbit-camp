using System.Text.Json;
using Archivation.Function.Data.Repositories;
using Archivation.Function.Models.DTos;
using Archivation.Function.Models.Enums;
using Archivation.Function.ServicesAbstractions;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Archivation.Function;

public class Archivation
{
    private readonly ILogger<Archivation> _logger;
    private readonly IBoardArchiveRepository _boardArchiveRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IArchivationLogsService _archivationLogsService;
    private readonly IMapper _mapper;

    public Archivation(ILogger<Archivation> logger, 
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

    [Function(nameof(Archivation))]
    public async Task Run(
        [ServiceBusTrigger(ArchivationConstants.ArchivationQueUerName, Connection = "AzureServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            var boardId = JsonSerializer.Deserialize<int>(message.Body.ToString());

            var board = await _boardArchiveRepository.GetBoardArchiveDataAsync(boardId);
            if (board != null)
            {
                await _archivationLogsService
                    .SaveArchivationLogAsync(boardId, ArchiveStatus.GotFromDatabase);
                
                var boardDto = _mapper.Map<BoardDto>(board);

                await _blobStorageService.UploadJsonAsync(boardId.ToString(), boardDto);

                await _boardArchiveRepository.MarkBoardAsArchived(boardId);

                await _boardArchiveRepository.DeleteBoardRelatedDataAsync(boardId);
            }
            else
            {
                await _archivationLogsService
                    .SaveArchivationLogAsync(boardId, ArchiveStatus.FailedToGetFromDataBase);
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
            _logger.LogError($"Failed archive board {message.Body}.");
            await messageActions.DeadLetterMessageAsync(
                message,
                null,
                "Archivation error",
                ex.ToString());
        }
    }
}