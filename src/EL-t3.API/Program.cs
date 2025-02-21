using EL_t3.API.Infrastructure;
using EL_t3.Application;
using EL_t3.Infrastructure;
using EL_t3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var developmentAllowedOrigin = "development_allowed_origin";

{
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
    builder.Services.AddCore();
    builder.Services.AddPersistence();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddGateways();
    builder.Logging.AddConsole();

    builder.Services.AddSwaggerGen(opt => { });
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: developmentAllowedOrigin,
            policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.WithOrigins("http://localhost:5173");
                });
    });
}

var app = builder.Build();

{
    // app.UseHttpsRedirection();
    app.UseCors(developmentAllowedOrigin);
    app.UseExceptionHandler("/Error");

    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(opt => { });
}

app.Run();

public partial class Program { }