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

internal sealed class GetPostEndpoint : MinimalEndpoint<PostsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("{slug}", Handle)
            .RequireScopes(AppScopes.Read);

        static async Task<Results<Ok<PostResponse>, ProblemHttpResult>> Handle(
            string slug,
            ClaimsPrincipal user,
            GetPost feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new GetPostRequest(slug, user.ToCaller()), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
