using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CardRepository :
    BaseRepository<Card, int>,
    ICardRepository
{
    public CardRepository(DbContext context) 
        : base(context)
    {}
}