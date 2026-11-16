using System.Security.Claims;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Diagnostics.Endpoints;

internal sealed class GetAuthorizedEndpoint : MinimalEndpoint<DiagnosticsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("authorized", Handle)
            .RequireAuthorization();

        static IResult Handle(ClaimsPrincipal user)
        {
            return TypedResults.Ok(new { Subject = user.GetUserId(), user.Identity?.Name });
        }
    }
}
