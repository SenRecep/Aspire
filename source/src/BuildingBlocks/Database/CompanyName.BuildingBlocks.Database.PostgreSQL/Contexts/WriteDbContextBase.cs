using CSharpEssentials.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL.Contexts;

public abstract class WriteDbContextBase<TContext> : BaseDbContext<TContext> where TContext : DbContext
{
    protected WriteDbContextBase(
        DbContextOptions<TContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.ConfigureEnumConventions();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TContext).Assembly,
            ConfigurationsFilter);
    }
    private static bool ConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Configurations.Write") ?? false;
}
