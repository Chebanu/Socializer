using Socializer.Domain.Models;

namespace Socializer.Application.Interfaces;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
