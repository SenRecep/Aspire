using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompanyName.BuildingBlocks.Database.Migrator;
public static class Extensions
{
    public static async Task MigrateAsync<TDbContext>(
        this IHost host)
        where TDbContext : DbContext
    {
        using IServiceScope serviceScope = host.Services.CreateScope();
        await using TDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        await EnsureDatabaseAsync(dbContext);
        await RunMigrationsAsync(dbContext);
    }

    private static async Task EnsureDatabaseAsync<TDbContext>(TDbContext dbContext)
        where TDbContext : DbContext
    {
        IRelationalDatabaseCreator dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
        IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            bool isExist = await dbCreator.ExistsAsync();
            if (isExist)
                return;
            await dbCreator.CreateAsync();
        });
    }

    private static async Task RunMigrationsAsync<TDbContext>(TDbContext dbContext)
        where TDbContext : DbContext
    {
        IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(() => dbContext.Database.MigrateAsync());
    }
}
