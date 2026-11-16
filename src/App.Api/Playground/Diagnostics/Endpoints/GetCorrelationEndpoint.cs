using System.Diagnostics;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.Core.Application;
using Aurore.Foundation.Core.RequestPipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Diagnostics.Endpoints;

internal sealed class GetCorrelationEndpoint : MinimalEndpoint<DiagnosticsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("correlation", Handle);

        static IResult Handle(RequestContext context)
        {
            return TypedResults.Ok(new
            {
                context.CorrelationId,
                context.TraceId,
                context.OriginalSpanId,
                context.ClientAddress,
                context.UserAgent,
                context.Host,
                context.UserId,
                CurrentSpanId = Activity.Current?.SpanId.ToString(),
                Version = AppVersion.Current,
            });
        }
    }
}
