using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class TokenProvider : ITokenProvider
{
    private readonly IOptions<JwtOptions> _options;

    public TokenProvider(IOptions<JwtOptions> options)
    {
        _options = options;
        AccessTokenLifetimeMinutes = _options.Value.ExpirationMinutes;
        RefreshTokenLifetimeDays = _options.Value.RefreshTokenExpirationDays;
    }
    
    public int AccessTokenLifetimeMinutes { get; }
    
    public int RefreshTokenLifetimeDays { get; }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));

        var credentials = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Value.Issuer,
            audience: _options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_options.Value.ExpirationMinutes)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token); 
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }
    
}