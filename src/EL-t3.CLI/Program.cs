using Cocona;
using EL_t3.Application;
using EL_t3.CLI.Commands;
using EL_t3.Infrastructure;
using EL_t3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = CoconaApp.CreateBuilder();

{
    builder.Services.AddHttpClients();
    builder.Services.AddGateways();
    builder.Services.AddPersistence((options) =>
        {
            options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        }); builder.Services.AddCore();
    builder.Logging.AddConsole();
}

var app = builder.Build();

{
    app.AddSubCommand("players", PlayerCommands.Commands);
    app.AddSubCommand("clubs", ClubCommands.Commands);
    app.Run();
}