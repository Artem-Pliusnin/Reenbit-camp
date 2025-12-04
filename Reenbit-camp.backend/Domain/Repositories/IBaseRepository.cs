using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IBaseRepository
{
    void SetContext(DbContext context);
}