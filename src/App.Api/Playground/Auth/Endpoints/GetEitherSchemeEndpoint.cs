using System.Security.Claims;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Auth.Endpoints;

internal sealed class GetEitherSchemeEndpoint : MinimalEndpoint<AuthResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("either", Handle).RequireScopes(AppScopes.Read);

        static IResult Handle(ClaimsPrincipal user)
        {
            return TypedResults.Ok(new { Caller = user.Identity?.Name, Scheme = user.Identity?.AuthenticationType });
        }
    }
}
