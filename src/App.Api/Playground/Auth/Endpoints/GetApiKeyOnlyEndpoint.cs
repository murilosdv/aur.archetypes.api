using System.Security.Claims;
using Aurore.Foundation.AspNetCore.Authentication.Constants;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Auth.Endpoints;

internal sealed class GetApiKeyOnlyEndpoint : MinimalEndpoint<AuthResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("api-key-only", (ClaimsPrincipal user) => TypedResults.Ok(new { Client = user.Identity?.Name }))
            .RequireAuthorization(policy => policy.AddAuthenticationSchemes(ApiKeyDefaults.AuthenticationScheme).RequireAuthenticatedUser())
            .RequireRoles("reader");
    }
}
