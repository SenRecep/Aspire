using Microsoft.AspNetCore.Http;

namespace CompanyName.BuildingBlocks.Presentation.SessionContexts;

public sealed class DefaultSessionContext(IHttpContextAccessor httpContextAccessor) : SessionContext(httpContextAccessor);
