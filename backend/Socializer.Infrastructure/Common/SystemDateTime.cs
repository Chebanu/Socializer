using Socializer.Application.Common.Interfaces;

namespace Socializer.Infrastructure.Common;

public class SystemDateTime : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
