using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Socializer.Application.Common.Interfaces;
using Socializer.Infrastructure.Common;

namespace Socializer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTime, SystemDateTime>();

        return services;
    }
}
