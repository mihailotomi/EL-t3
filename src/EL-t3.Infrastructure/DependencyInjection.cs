using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Application.Common.Interfaces.Repository;
using EL_t3.Infrastructure.Database;
using EL_t3.Infrastructure.Database.Repository;
using EL_t3.Infrastructure.Gateway;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddGateways(this IServiceCollection services)
    {
        services.AddSingleton<EuroleagueApiGateway>();
        services.AddSingleton<IClubBySeasonGateway>(sp => sp.GetService<EuroleagueApiGateway>()!);
        services.AddSingleton<IPlayerBySeasonGateway>(sp => sp.GetService<EuroleagueApiGateway>()!);
        services.AddSingleton<IPlayerByClubGateway, ProballersGateway>();
        services.AddSingleton<IAllClubsGateway, ProballersGateway>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IPlayerSeasonRepository, PlayerSeasonRepository>();
        services.AddScoped<IAppDatabaseContext, AppDatabaseContext>();

        return services;
    }
}
