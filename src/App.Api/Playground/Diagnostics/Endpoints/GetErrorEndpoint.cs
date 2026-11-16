using System;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Diagnostics.Endpoints;

internal sealed class GetErrorEndpoint : MinimalEndpoint<DiagnosticsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("error", Handle);

        static IResult Handle()
        {
            throw new InvalidOperationException("Test exception from the diagnostics resource.");
        }
    }
}
