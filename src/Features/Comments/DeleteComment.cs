using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Comments;

public sealed record DeleteCommentRequest(Guid PostId, Guid CommentId);

public sealed class DeleteComment(AppDbContext db) : Feature<DeleteCommentRequest>
{
    protected override async ValueTask<Result> ExecuteAsync(
        DeleteCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await db.Comments.AsTracking().SingleOrDefaultAsync(x => x.Id == request.CommentId && x.PostId == request.PostId, cancellationToken);

        if (comment is null)
            return AppErrors.Comments.NotFound;

        db.Comments.Remove(comment);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
