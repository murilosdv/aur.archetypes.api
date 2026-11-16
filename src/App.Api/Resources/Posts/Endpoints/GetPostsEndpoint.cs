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
using Aurore.Foundation.AspNetCore.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Posts.Endpoints;

internal sealed class GetPostsEndpoint : MinimalEndpoint<PostsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet(string.Empty, Handle)
            .RequireScopes(AppScopes.Read)
            .ProducesPage<PostSummaryResponse>();

        static async Task<Results<Ok<Page<PostSummaryResponse>>, ProblemHttpResult>> Handle(
            [AsParameters] SearchPostsFilter filter,
            ClaimsPrincipal user,
            SearchPosts feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new SearchPostsRequest(filter, user.ToCaller()), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
