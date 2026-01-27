using Application.Abstractions.Messaging;

namespace Application.FAQChat.Commands.ImportFile;

public sealed record ImportFileCommand(
    Stream Content,
    string FileName) : ICommand;