using EL_t3.Application.Common.Behaviors;
using EL_t3.Application.Player.Helpers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddScoped<PlayerSeedHelper>();

        return services;
    }
}
