using Domain.Entities;
using Domain.Repositories;
using Persistence.Database;

namespace Persistence.Repositories;

public class CardLabelsRepository: 
    BaseRepository<CardLabel, int>, 
    ICardLabelsRepository
{
    public CardLabelsRepository(TrelloAppDbContext context) 
        : base(context)
    {}
}