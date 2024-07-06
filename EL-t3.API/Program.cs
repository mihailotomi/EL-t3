using Microsoft.EntityFrameworkCore;
using EL_t3.Infrastructure.Database;
using EL_t3.Core;
using EL_t3.Infrastructure;
using EL_t3.Core.Exceptions.Middleware;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddDbContext<AppDatabaseContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddCore();
    builder.Services.AddRepositories();
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

{
    app.UseHttpsRedirection(); 
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();