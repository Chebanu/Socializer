using Microsoft.Extensions.DependencyInjection;

namespace Socializer.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers everything owned by the Application layer (use cases, validators, mapping profiles).
    /// Intentionally empty for now — Sprint 1+ modules add their registrations here as they land,
    /// keeping Socializer.Api's composition root to a single call per layer.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
