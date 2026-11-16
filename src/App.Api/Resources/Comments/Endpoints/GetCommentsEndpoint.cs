using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Comments;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Aurore.Foundation.AspNetCore.Pagination.Extensions;
using Aurore.Foundation.Core.Pagination.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Comments.Endpoints;

internal sealed class GetCommentsEndpoint : MinimalEndpoint<CommentsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet(string.Empty, Handle)
            .RequireScopes(AppScopes.Read)
            .ProducesCursorPage<CommentSummaryResponse>();

        static async Task<Results<Ok<CursorPage<CommentSummaryResponse>>, ProblemHttpResult>> Handle(
            [AsParameters] ListCommentsRequest request,
            ListComments feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(request, cancellationToken);

            return result.ToHttpResult();
        }
    }
}
