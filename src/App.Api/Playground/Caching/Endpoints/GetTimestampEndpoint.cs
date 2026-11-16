using System;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Aurore.Archetypes.Api.App.Api.Playground.Caching.Endpoints;

internal sealed class GetTimestampEndpoint : MinimalEndpoint<CachingResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("timestamp", (TimeProvider time) => TypedResults.Ok(new { Timestamp = time.GetUtcNow() }))
            .CacheOutput("short");
    }
}
