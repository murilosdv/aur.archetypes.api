using System;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.AspNetCore.RequestPipeline.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Idempotency.Endpoints;

internal sealed class PostPaymentEndpoint : MinimalEndpoint<IdempotencyResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("payments", Handle)
            .WithIdempotency(required: true);

        static IResult Handle(HttpContext context)
        {
            var id = Guid.NewGuid();
            context.Response.Headers["X-Payment-Provider"] = "playground";

            return TypedResults.Created($"/v1/playground/idempotency/payments/{id}", new { Id = id });
        }
    }
}
