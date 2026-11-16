using Aurore.Archetypes.Api.App.Api.Playground.Results.Shared;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Results.Endpoints;

internal sealed class DeletePlaygroundOrderEndpoint : MinimalEndpoint<ResultsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder.MapDelete("orders/{id:int}", Handle);

        static Results<NoContent, ProblemHttpResult> Handle(int id)
        {
            return PlaygroundOrders.Delete(id).ToHttpResult();
        }
    }
}
