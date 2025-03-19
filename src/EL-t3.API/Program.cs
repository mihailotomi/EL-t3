using EL_t3.API.Auth;
using EL_t3.API.Infrastructure;
using EL_t3.Application;
using EL_t3.Infrastructure;
using EL_t3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddDbContext<AppDatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddCore();
    builder.Services.AddPersistence();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddHttpClients();
    builder.Services.AddGateways();
    builder.Services.AddAuth(builder.Configuration);
    builder.Logging.AddConsole();

    builder.Services.AddSwaggerGen(opt => { });

    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowedOriginsPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseCors("AllowedOriginsPolicy");
    app.UseExceptionHandler("/Error");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(opt => { });
}

app.Run();

public partial class Program { }