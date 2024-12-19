using System.Data;
using CSharpEssentials;

namespace CompanyName.BuildingBlocks.Application.Shared.Abstractions;
public interface IUnitOfWork
{
    IDbTransaction BeginTransaction();
    Task<Result> ExecuteTransactionAsync(
        Func<Task> func,
        Func<Task>? rollbackFunc = null);
    Task<Result<T>> ExecuteTransactionAsync<T>(
        Func<Task<Result<T>>> func,
        Func<Task>? rollbackFunc = null);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
