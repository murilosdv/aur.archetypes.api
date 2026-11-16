using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed record GetPostRequest(string Slug, Caller Caller);

public sealed class GetPost(AppDbContext db) : Feature<GetPostRequest, PostResponse>
{
    protected override async ValueTask<Result<PostResponse>> ExecuteAsync(
        GetPostRequest request,
        CancellationToken cancellationToken)
    {
        var caller = request.Caller;

        var post = await db.Posts
            .Where(x => x.Slug == request.Slug)
            .Where(x => x.Status == PostStatus.Published || caller.IsAdmin || x.Author.Subject == caller.Subject)
            .Select(x => new PostResponse(x.Id, x.Slug, x.Title, x.Body, x.Status, x.Author.Handle, x.PublishedAt, x.CreatedAt, x.UpdatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        return post is null ? AppErrors.Posts.NotFound : post;
    }
}
