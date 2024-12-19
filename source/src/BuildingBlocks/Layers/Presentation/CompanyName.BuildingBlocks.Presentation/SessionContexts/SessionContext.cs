using System.Security.Claims;
using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Shared.Abstractions;
using CompanyName.BuildingBlocks.Application.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace CompanyName.BuildingBlocks.Presentation.SessionContexts;

public abstract class SessionContext(IHttpContextAccessor httpContextAccessor) : ISessionContext
{
    private HttpContext? Context => httpContextAccessor.HttpContext;

    private bool CheckClaimValue(string type) =>
        Context?.User.HasClaim(x => x.Type == type) ?? false;

    private bool CheckHeaderValue(string key) =>
        Context?.Request.Headers.ContainsKey(key) ?? false;

    private Maybe<string> GetClaimValue(string type) =>
        Context?.User.FindFirst(x => x.Type == type)?.Value;

    private Maybe<string> GetHeaderValue(string key) => Context?.Request.Headers[key].ToString();


    public bool IsAuthenticated => Context?.User.Identity?.IsAuthenticated ?? false;

    public Maybe<string> UserId => CheckClaimValue(ClaimTypes.NameIdentifier) ? GetClaimValue(ClaimTypes.NameIdentifier) : Maybe.None;

    public Maybe<string> TenantId => GetTenantId();

    public Maybe<string> AccessToken => Context?
            .Request
            .Headers[HeaderNames.Authorization]
            .ToString()
            .Replace($"{JwtBearerDefaults.AuthenticationScheme} ", "");

    private Maybe<string> GetTenantId()
    {
        if (CheckHeaderValue(CustomHeaderNames.TenantId))
        {
            return GetHeaderValue(CustomHeaderNames.TenantId);
        }

        if (CheckClaimValue(CustomClaimTypes.TenantId))
        {
            return GetClaimValue(CustomClaimTypes.TenantId);
        }

        return Maybe.None;
    }
}
