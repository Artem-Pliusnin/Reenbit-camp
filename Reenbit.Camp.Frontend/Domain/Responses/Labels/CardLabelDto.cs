using System.Reflection.Emit;

namespace Domain.Responses.Labels;

public record CardLabelDto(
    int Id,
    LabelDto Label);