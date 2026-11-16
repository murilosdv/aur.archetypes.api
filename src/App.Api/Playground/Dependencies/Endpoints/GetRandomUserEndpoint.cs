using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Dependencies.Endpoints;

internal sealed class GetRandomUserEndpoint : MinimalEndpoint<DependenciesResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapGet("random-user", HandleAsync);

        static async Task<IResult> HandleAsync(IHttpClientFactory clientFactory, CancellationToken cancellationToken)
        {
            var http = clientFactory.CreateClient(HttpDependencyNames.RandomUser);

            using var response = await http.GetAsync("api/?results=1", cancellationToken);

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            return TypedResults.Content(body, "application/json", statusCode: (int)response.StatusCode);
        }
    }
}
