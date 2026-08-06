using Socializer.Domain.Models;

namespace Socializer.Application.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(User user);
    public string GenerateRefreshToken();
    public string HashRefreshToken(string rawToken);
}