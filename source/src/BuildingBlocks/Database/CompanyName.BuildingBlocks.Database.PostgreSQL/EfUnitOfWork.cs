using CSharpEssentials;
using CSharpEssentials.Json;
using CompanyName.BuildingBlocks.Application.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;

namespace CompanyName.BuildingBlocks.Database.PostgreSQL;
public sealed class EfUnitOfWork<TContext>(
    TContext context,
    ILogger<EfUnitOfWork<TContext>> logger) : IUnitOfWork where TContext : DbContext
{
    public IDbTransaction BeginTransaction()
    {
        IDbContextTransaction transaction = context.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    public async Task<Result> ExecuteTransactionAsync(Func<Task> func, Func<Task>? rollbackFunc = null)
    {
        IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await func();
                await transaction.CommitAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Transaction Error");
                await transaction.RollbackAsync();

                if (rollbackFunc is null)
                    return Error.Exception(ex);

                try
                {
                    await rollbackFunc();
                }
                catch (Exception rollbackEx)
                {
                    logger.LogError(rollbackEx, "Rollback Error");
                    return Error.Exception(rollbackEx);
                }

                return Error.Exception(ex);
            }
        });
    }

    public async Task<Result<T>> ExecuteTransactionAsync<T>(Func<Task<Result<T>>> func, Func<Task>? rollbackFunc = null)
    {
        IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                Result<T> result = await func();
                if (result.IsFailure)
                {
                    logger.LogWarning("Task result is failure. Rolling back transaction. {Result}", result.ConvertToJson());
                    await transaction.RollbackAsync();
                    return result;
                }
                await transaction.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Transaction Error");
                await transaction.RollbackAsync();

                if (rollbackFunc is null)
                    return Error.Exception(ex);

                try
                {
                    await rollbackFunc();
                }
                catch (Exception rollbackEx)
                {
                    logger.LogError(rollbackEx, "Rollback Error");
                    return Error.Exception(rollbackEx);
                }

                return Error.Exception(ex);
            }
        });
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
