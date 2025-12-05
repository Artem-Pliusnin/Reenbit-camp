using Domain.Entities;

namespace Application.Abstractions.Services;

public interface ITokenProvider
{
    string CreateToken(User user);
    string GenerateRefreshToken();
}