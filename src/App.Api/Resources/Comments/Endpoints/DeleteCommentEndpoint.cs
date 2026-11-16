using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Constants;
using Aurore.Archetypes.Api.Features.Comments;
using Aurore.Foundation.AspNetCore.Authentication.Extensions;
using Aurore.Foundation.AspNetCore.Extensions;
using Aurore.Foundation.AspNetCore.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Aurore.Archetypes.Api.App.Api.Resources.Comments.Endpoints;

internal sealed class DeleteCommentEndpoint : MinimalEndpoint<CommentsResource>
{
    public override RouteHandlerBuilder Map(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("{commentId:guid}", Handle)
            .RequireRoles("admin")
            .RequireScopes(AppScopes.Write);

        static async Task<Results<NoContent, ProblemHttpResult>> Handle(
            Guid postId,
            Guid commentId,
            DeleteComment feature,
            CancellationToken cancellationToken)
        {
            var result = await feature.HandleAsync(new DeleteCommentRequest(postId, commentId), cancellationToken);

            return result.ToHttpResult();
        }
    }
}
