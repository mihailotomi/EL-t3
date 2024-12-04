using Cocona;
using EL_t3.Application;
using EL_t3.CLI.Commands;
using EL_t3.Infrastructure;
using EL_t3.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = CoconaApp.CreateBuilder();

{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    builder.Services.AddHttpClient("euroleague-api", client =>
    {
        client.BaseAddress = new Uri("https://api-live.euroleague.net");
    });
    builder.Services.AddHttpClient("proballers", client =>
    {
        client.BaseAddress = new Uri("https://www.proballers.com");
    });
    builder.Services.AddDbContext<AppDatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddGateways();
    builder.Services.AddCore();
    builder.Services.AddRepositories();
    builder.Logging.AddConsole();
}

var app = builder.Build();

{
    app.AddSubCommand("players", PlayerCommands.Commands);
    app.AddSubCommand("clubs", ClubCommands.Commands);
    app.Run();
}