using Domain.DTOs.CardLabels;

namespace Domain.DTOs.Cards;

public class UpdatedCardLabelsDto
{
    public int CardId {get;set;}
    
    public List<CardLabelDto> Labels {get;set;}
}