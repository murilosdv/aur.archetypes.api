using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.App.Api.Resources.Shared;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Posts;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;

internal sealed class PublishPostEndpoint : MinimalEndpoint<PostsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("{id:guid}/publish", Handle)
            .RequireAnyRole("editor", "admin")
            .RequireScopes(AppScopes.Write);

        static async Task<Results<Ok<PostResponse>, ProblemHttpResult>> Handle(
            Guid id,
            ClaimsPrincipal user,
            PublishPost feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new PublishPostRequest(id, user.ToCaller()), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
