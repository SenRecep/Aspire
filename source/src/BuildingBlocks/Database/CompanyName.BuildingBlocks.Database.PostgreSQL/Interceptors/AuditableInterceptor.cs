using CSharpEssentials;
using CSharpEssentials.Interfaces;
using CSharpEssentials.Time;
using CompanyName.BuildingBlocks.Application.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL.Interceptors;

internal sealed class AuditableInterceptor(
    IDateTimeProvider dateTimeProvider,
    IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            UpdateAuditableEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private string GetUserId(IUserContext userContext)
    {
        if (userContext.UserId.HasValue)
            return userContext.UserId.Value;
        return "system";
    }
    private void UpdateAuditableEntities(DbContext context)
    {
        IServiceProvider provider = serviceScopeFactory.CreateScope().ServiceProvider;
        IUserContext userContext = provider.GetRequiredService<IUserContext>();
        string userId = GetUserId(userContext);
        DateTimeOffset now = dateTimeProvider.UtcNow;
        foreach (EntityEntry entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added && entry.Entity is ICreationAudit creationAudit)
                creationAudit.SetCreatedInfo(now, userId);

            if (entry.State == EntityState.Modified && entry.Entity is IModificationAudit modificationAudit)
                modificationAudit.SetUpdatedInfo(now, userId);

            if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable deletionAudit && deletionAudit.IsHardDeleted.IsFalse())
            {
                deletionAudit.MarkAsDeleted(now, userId);
                entry.State = EntityState.Modified;
            }
        }
    }
}
