using System;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.RateLimiting.Endpoints;

internal sealed class GetLimitedEndpoint : MinimalEndpoint<RateLimitingResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("limited", (TimeProvider time) => TypedResults.Ok(new { Timestamp = time.GetUtcNow() }))
            .RequireRateLimiting("tests");
    }
}
