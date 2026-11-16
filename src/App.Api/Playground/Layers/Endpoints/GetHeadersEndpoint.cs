using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Layers.Endpoints;

internal sealed class GetHeadersEndpoint : MinimalEndpoint<LayersResource>
{
    public override string Summary => "Shows how app, group and endpoint conventions stack";

    public override string Description => "Anonymous endpoint returning headers written by each level.";

    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("headers", () => TypedResults.Ok(new { Message = "Check the X-Level-* and X-Configured-By response headers." }))
            .AllowAnonymous()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .AddEndpointFilter((context, next) =>
            {
                context.HttpContext.Response.Headers[LayersResource.ConfiguredByHeader] = "endpoint";
                context.HttpContext.Response.Headers["X-Level-Endpoint"] = "true";

                return next(context);
            });
    }
}
