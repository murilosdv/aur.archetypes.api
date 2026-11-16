using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Clients.Email;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Core.Settings;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Features.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed record PublishPostRequest(Guid PostId, Caller Caller);

public sealed class PublishPost(
    AppDbContext db,
    TimeProvider time,
    IEmailSender email,
    EmailOptions emailOptions,
    ILogger<PublishPost> logger)
    : Feature<PublishPostRequest, PostResponse>
{
    protected override async ValueTask<Result<PostResponse>> ExecuteAsync(
        PublishPostRequest request,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts.AsTracking().Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);

        if (post is null)
            return AppErrors.Posts.NotFound;

        if (request.Caller.IsAdmin is false && post.Author.Subject != request.Caller.Subject)
            return AppErrors.Posts.NotTheAuthor;

        if (post.Status is PostStatus.Published)
            return AppErrors.Posts.AlreadyPublished;

        post.Status = PostStatus.Published;
        post.PublishedAt = time.GetUtcNow();

        await db.SaveChangesAsync(cancellationToken);

        await email.TrySendAsync(ModerationEmails.PostPublished(emailOptions, post.Title, post.Slug, ModerationEmails.Person(post.Author.Handle, post.Author.DisplayName)), logger, cancellationToken);

        return new PostResponse(post.Id, post.Slug, post.Title, post.Body, post.Status, post.Author.Handle, post.PublishedAt, post.CreatedAt, post.UpdatedAt);
    }
}
