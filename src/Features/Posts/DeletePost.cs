using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed record DeletePostRequest(Guid PostId);

public sealed class DeletePost(AppDbContext db) : Feature<DeletePostRequest>
{
    protected override async ValueTask<Result> ExecuteAsync(
        DeletePostRequest request,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts.AsTracking().SingleOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);

        if (post is null)
            return AppErrors.Posts.NotFound;

        db.Posts.Remove(post);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
