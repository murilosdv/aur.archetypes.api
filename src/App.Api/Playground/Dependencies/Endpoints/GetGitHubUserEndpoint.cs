using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.GitHub;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Dependencies.Endpoints;

internal sealed class GetGitHubUserEndpoint : MinimalEndpoint<DependenciesResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("github/{user}", HandleAsync);

        static async Task<IResult> HandleAsync(string user, IGitHubClient gitHub, CancellationToken cancellationToken)
        {
            var result = await gitHub.GetUserAsync(user, cancellationToken);

            return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
        }
    }
}
