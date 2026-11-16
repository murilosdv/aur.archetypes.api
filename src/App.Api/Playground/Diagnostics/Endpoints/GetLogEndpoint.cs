using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Aurore.Archetypes.Api.App.Api.Playground.Diagnostics.Endpoints;

internal sealed class GetLogEndpoint : MinimalEndpoint<DiagnosticsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("log", Handle);

        static IResult Handle(ILogger<GetLogEndpoint> logger)
        {
            logger.LogInformation("Playground log line written inside the request.");

            return TypedResults.NoContent();
        }
    }
}
