using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace EL_t3.API.Auth;

public static class DependencyInjection
{
    public static AuthenticationBuilder AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenIdConnectOptions>(configuration.GetSection(nameof(OpenIdConnectOptions)));

        return services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "EL-t3-AC";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;

                options.Events.OnRedirectToAccessDenied += context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToLogin += context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            })
            .AddOpenIdConnect(options =>
            {
                options.CorrelationCookie.SameSite = SameSiteMode.None;
                options.NonceCookie.SameSite = SameSiteMode.None;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;

                configuration.Bind(nameof(OpenIdConnectOptions), options);
            });
    }
}
