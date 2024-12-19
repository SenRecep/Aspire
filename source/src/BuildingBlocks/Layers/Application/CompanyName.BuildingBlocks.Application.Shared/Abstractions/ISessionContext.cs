using CSharpEssentials;

namespace CompanyName.BuildingBlocks.Application.Shared.Abstractions;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    Maybe<string> UserId { get; }
    Maybe<string> AccessToken { get; }
}

public interface ITenantContext
{
    Maybe<string> TenantId { get; }
}

public interface ISessionContext : IUserContext, ITenantContext;
