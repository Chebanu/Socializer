using Socializer.Domain.Common;
using Socializer.Domain.Exceptions;

namespace Socializer.Domain.Models;

public class RefreshToken : BaseAuditableEntity
{
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool Revoked { get; private set; }

    private RefreshToken()
    {
        // EF Core materialization
    }

    public static RefreshToken Issue(Guid userId, string tokenHash, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(tokenHash))
        {
            throw new DomainException("Token hash is required.");
        }

        if (expiresAt <= DateTime.UtcNow)
        {
            throw new DomainException("Expiry must be in the future.");
        }

        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt
        };
    }

    public bool IsActive => !Revoked && ExpiresAt > DateTime.UtcNow;

    public void Revoke()
    {
        if (Revoked)
        {
            throw new DomainException("Token is already revoked.");
        }

        Revoked = true;
    }
}
