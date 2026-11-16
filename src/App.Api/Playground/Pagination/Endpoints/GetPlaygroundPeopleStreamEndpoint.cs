using Aurore.Archetypes.Api.App.Api.Playground.Pagination.Shared;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.AspNetCore.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Playground.Pagination.Endpoints;

internal sealed class GetPlaygroundPeopleStreamEndpoint : MinimalEndpoint<PaginationResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("people/stream", Handle)
            .ProducesCursorPage<PlaygroundPerson>();

        static Results<Ok<CursorPage<PlaygroundPerson>>, ProblemHttpResult> Handle(
            [AsParameters] StreamPlaygroundPeopleRequest request,
            PaginationOptions options)
        {
            return PlaygroundPeople.Stream(request, options).ToHttpResult();
        }
    }
}
