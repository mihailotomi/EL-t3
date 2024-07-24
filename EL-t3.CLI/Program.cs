﻿using Cocona;
using EL_t3.CLI.Commands;
using EL_t3.Infrastructure;
using EL_t3.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

{
    builder.Services.AddHttpClient("euroleague-api", client =>
    {
        client.BaseAddress = new Uri("https://api-live.euroleague.net");
    });
    builder.Services.AddHttpClient("proballers", client =>
    {
        client.BaseAddress = new Uri("https://www.proballers.com");
    });
    builder.Services.AddGateways();
    builder.Services.AddRepositories();
    builder.Services.AddDbContext<AppDatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

{
    app.AddCommands<ClubCommands>();
    app.AddCommands<PlayerCommands>();

    app.Run();
}