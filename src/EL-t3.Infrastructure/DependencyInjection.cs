using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Infrastructure.Gateway;
using EL_t3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.Infrastructure;

public static class DependencyInjection
{
    public class PersistenceOptions
    {
        public string? ConnectionString { get; set; }
    }

    public static IServiceCollection AddGateways(this IServiceCollection services)
    {
        services.AddSingleton<EuroleagueApiGateway>();
        services.AddSingleton<IClubBySeasonGateway>(sp => sp.GetService<EuroleagueApiGateway>()!);
        services.AddSingleton<IPlayerBySeasonGateway>(sp => sp.GetService<EuroleagueApiGateway>()!);
        services.AddSingleton<IPlayerByClubGateway, ProballersGateway>();
        services.AddSingleton<IAllClubsGateway, ProballersGateway>();

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient("euroleague-api", client =>
    {
        client.BaseAddress = new Uri("https://api-live.euroleague.net");
    });
        services.AddHttpClient("proballers", client =>
        {
            client.BaseAddress = new Uri("https://www.proballers.com");
        });

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, Action<PersistenceOptions>? options = null)
    {
        var persistenceOptions = new PersistenceOptions();
        if (options != null)
        {
            options(persistenceOptions);
        }

        if (string.IsNullOrEmpty(persistenceOptions.ConnectionString))
        {
            throw new ArgumentException("Connection string is required", nameof(options));
        }

        services.AddDbContext<AppDatabaseContext>(options =>
            {
                options.UseSnakeCaseNamingConvention();
                options.UseNpgsql(persistenceOptions.ConnectionString, e =>
                {
                    e.MigrationsAssembly(typeof(AppDatabaseContext).Assembly.FullName);
                    e.MigrationsHistoryTable("__EFMigrationsHistory", AppDatabaseContext.SchemaName);
                });
            });
        services.AddScoped<IAppDatabaseContext, AppDatabaseContext>();

        return services;
    }
}
