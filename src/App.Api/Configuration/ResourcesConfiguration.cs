using Aurore.Archetypes.Api.App.Api.Playground.Layers;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.AspNetCore.MinimalApis.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Aurore.Archetypes.Api.App.Api.Configuration;

internal static class ResourcesConfiguration
{
    internal static WebApplication MapAppResources(this WebApplication app, MinimalEndpointCatalog endpoints)
    {
        app.MapStandardMinimalEndpoints(endpoints, api => api
            .ProducesProblem(StatusCodes.Status503ServiceUnavailable)

            // Layering example (see Playground.http); remove when no longer needed.
            .AddEndpointFilter((context, next) =>
            {
                context.HttpContext.Response.Headers[LayersResource.ConfiguredByHeader] = "app";
                context.HttpContext.Response.Headers["X-Level-App"] = "true";

                return next(context);
            }));

        return app;
    }
}
