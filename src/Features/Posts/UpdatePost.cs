using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed record UpdatePostRequest(Guid PostId, string? Title, string? Body, Caller Caller);

public sealed class UpdatePost(AppDbContext db) : Feature<UpdatePostRequest, PostResponse>
{
    protected override async ValueTask<Result<PostResponse>> ExecuteAsync(
        UpdatePostRequest request,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts.AsTracking().Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);

        if (post is null)
            return AppErrors.Posts.NotFound;

        if (request.Caller.IsAdmin is false && post.Author.Subject != request.Caller.Subject)
            return AppErrors.Posts.NotTheAuthor;

        post.Title = request.Title ?? post.Title;
        post.Body = request.Body ?? post.Body;

        await db.SaveChangesAsync(cancellationToken);

        return new PostResponse(post.Id, post.Slug, post.Title, post.Body, post.Status, post.Author.Handle, post.PublishedAt, post.CreatedAt, post.UpdatedAt);
    }
}
