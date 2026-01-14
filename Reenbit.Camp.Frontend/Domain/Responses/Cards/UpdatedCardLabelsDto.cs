using Domain.Responses.Labels;

namespace Domain.Responses.Cards;

public record UpdatedCardLabelsDto(
    int CardId,
    List<CardLabelDto> Labels);