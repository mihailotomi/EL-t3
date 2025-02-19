using EL_t3.API.Tests.Integration.Common;
using EL_t3.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.API.Tests.Integration.Controllers;

public abstract class BaseControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime{
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