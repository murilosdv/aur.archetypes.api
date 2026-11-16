using System;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.AspNetCore.RequestPipeline.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Idempotency.Endpoints;

internal sealed class PostOrderEndpoint : MinimalEndpoint<IdempotencyResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("orders", (TimeProvider time) => TypedResults.Ok(new { Id = Guid.NewGuid(), CreatedAt = time.GetUtcNow() }))
            .WithIdempotency();
    }
}
