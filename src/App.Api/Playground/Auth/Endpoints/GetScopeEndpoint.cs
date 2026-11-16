using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Auth.Endpoints;

internal sealed class GetScopeEndpoint : MinimalEndpoint<AuthResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("scope", () => TypedResults.Ok(new { Granted = AppScopes.Read }))
            .RequireScopes(AppScopes.Read);
    }
}
