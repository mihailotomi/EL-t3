using EL_t3.Application;
using EL_t3.Application.Common.Exceptions.Middleware;
using EL_t3.Infrastructure;
using EL_t3.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var developmentAllowedOrigin = "development_allowed_origin";

{
    builder.Services.AddDbContext<AppDatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddCore();
    builder.Services.AddRepositories();
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
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
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseCors(developmentAllowedOrigin);

    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();