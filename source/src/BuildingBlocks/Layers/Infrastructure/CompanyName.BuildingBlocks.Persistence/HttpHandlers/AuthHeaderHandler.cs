using System.Net.Http.Headers;
using CSharpEssentials;
using CompanyName.BuildingBlocks.Application.Shared.Abstractions;
using CompanyName.BuildingBlocks.Application.Shared.Constants;

namespace CompanyName.BuildingBlocks.Persistence.HttpHandlers;
public sealed class AuthHeaderHandler(
    ISessionContext sessionContext) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Maybe<string> accessToken = sessionContext.AccessToken;
        Maybe<string> tanentId = sessionContext.TenantId;

        accessToken.Execute(token => request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token));
        tanentId.Execute(tenantId => request.Headers.Add(CustomHeaderNames.TenantId, tenantId));

        return base.SendAsync(request, cancellationToken);
    }
}
