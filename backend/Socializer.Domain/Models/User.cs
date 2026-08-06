using Socializer.Domain.Common;
using Socializer.Domain.Exceptions;

namespace Socializer.Domain.Models;

public class User : BaseAuditableEntity
{
    private const int MINIMUM_AGE = 18;

    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool EmailConfirmed { get; private set; }
    public DateTime BirthDate { get; private set; }
    public bool IsBanned { get; private set; }

    private User()
    {
        // EF Core materialization
    }

    public static User Register(string email, string passwordHash, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new DomainException("Password hash is required.");
        }

        if (GetAge(birthDate) < MINIMUM_AGE)
        {
            throw new DomainException($"User must be at least {MINIMUM_AGE} years old.");
        }

        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            BirthDate = birthDate
        };
    }

    public void ConfirmEmail()
    {
        if (EmailConfirmed)
        {
            throw new DomainException("Email is already confirmed.");
        }

        EmailConfirmed = true;
    }

    public void Ban()
    {
        if (IsBanned)
        {
            throw new DomainException("User is already banned.");
        }

        IsBanned = true;
    }

    public void Unban()
    {
        if (!IsBanned)
        {
            throw new DomainException("User is not banned.");
        }

        IsBanned = false;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new DomainException("Password hash is required.");
        }

        PasswordHash = newPasswordHash;
    }

    private static int GetAge(DateTime birthDate)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
