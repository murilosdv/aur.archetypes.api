using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Layers.Endpoints;

internal sealed class GetHeadersV2Endpoint : MinimalEndpoint<LayersResource>
{
    public override int Version => 2;

    public override string Name => "GetHeadersV2";

    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("headers", () => TypedResults.Ok(new { Message = "Version 2 of the same route." }))
            .AllowAnonymous();
    }
}
