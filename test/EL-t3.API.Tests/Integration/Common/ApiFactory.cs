using System.Data.Common;
using EL_t3.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace EL_t3.API.Tests.Integration.Common;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("t3_test_db")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private static Respawner _respawner = null!;
    private DbConnection _dbConnection = default!;
    private static bool _isInitialized;
    public ServiceProvider ServiceProvider { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDatabaseContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDatabaseContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            var sp = services.BuildServiceProvider();
            ServiceProvider = sp;
        });
    }

    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            await _dbContainer.StartAsync();
            _isInitialized = true;

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
            await db.Database.MigrateAsync();

            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            _dbConnection.Open();

            _respawner = Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        await ResetDatabaseAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}

