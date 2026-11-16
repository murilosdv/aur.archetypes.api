using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Posts;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;

internal sealed class DeletePostEndpoint : MinimalEndpoint<PostsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("{id:guid}", Handle)
            .RequireRoles("admin")
            .RequireScopes(AppScopes.Write);

        static async Task<Results<NoContent, ProblemHttpResult>> Handle(
            Guid id,
            DeletePost feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new DeletePostRequest(id), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
