using EL_t3.Core.Interfaces.Gateway;
using EL_t3.Core.Interfaces.Repository;
using EL_t3.Infrastructure.Database.Repository;
using EL_t3.Infrastructure.Gateway;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddGateways(this IServiceCollection services)
    {
        services.AddSingleton<EuroleagueApiGateway>();
        services.AddSingleton<IClubGateway>(sp => sp.GetService<EuroleagueApiGateway>()!);
        services.AddSingleton<IPlayerBySeasonGateway>(sp => sp.GetService<EuroleagueApiGateway>()!);

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IPlayerSeasonRepository, PlayerSeasonRepository>();

        return services;
    }
}
