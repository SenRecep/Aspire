using CSharpEssentials.EntityFrameworkCore;
using CompanyName.BuildingBlocks.Database.PostgreSQL.Contexts;
using CompanyName.Services.CategoryService.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Services.CategoryService.Persistence.EntityFrameworkCore.Contexts;
public sealed class ApplicationWriteDbContext : WriteDbContextBase<ApplicationWriteDbContext>
{
    public ApplicationWriteDbContext(
        DbContextOptions<ApplicationWriteDbContext> options,
        IServiceScopeFactory serviceScopeFactory) : base(options, serviceScopeFactory)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySoftDeleteQueryFilter();
        // modelBuilder.AddMassTransitOutboxEntities();
    }
}
