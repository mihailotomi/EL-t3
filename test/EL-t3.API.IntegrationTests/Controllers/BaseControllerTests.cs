using EL_t3.API.Tests.Common;
using EL_t3.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.API.Tests.Controllers;

public abstract class BaseControllerTests : IAsyncLifetime
{
    protected readonly HttpClient client;
    protected Func<Task> resetDatabase;
    protected AppDatabaseContext dbContext = default!;
    protected IServiceScope scope = default!;

    public BaseControllerTests(ApiFactory factory)
    {
        client = factory.CreateClient();
        var serviceProvider = factory.ServiceProvider;
        resetDatabase = factory.ResetDatabaseAsync;
        scope = serviceProvider.CreateScope();
        dbContext = scope.ServiceProvider.GetService<AppDatabaseContext>()!;
    }
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        scope.Dispose();
        await resetDatabase();
    }
}