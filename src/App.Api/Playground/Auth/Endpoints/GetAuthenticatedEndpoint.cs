using System.Linq;
using System.Security.Claims;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Auth.Endpoints;

internal sealed class GetAuthenticatedEndpoint : MinimalEndpoint<AuthResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("authenticated", Handle);

        static IResult Handle(ClaimsPrincipal user)
        {
            return TypedResults.Ok(new
            {
                Subject = user.GetUserId(),
                Claims = user.Claims.Select(x => new { x.Type, x.Value }),
            });
        }
    }
}
