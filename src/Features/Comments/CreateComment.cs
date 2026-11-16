using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Errors;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Comments;

public sealed record CreateCommentRequest(string Subject, Guid PostId, string Body);

public sealed record CommentResponse(Guid Id, Guid PostId, string Body, string AuthorHandle, DateTimeOffset CreatedAt);

internal sealed class CreateCommentAuthorValidator : FeatureValidator<CreateCommentRequest>
{
    public CreateCommentAuthorValidator(AppDbContext db)
    {
        RuleFor(x => x.Subject)
            .MustAsync((subject, cancellationToken) => db.Authors.AnyAsync(x => x.Subject == subject, cancellationToken))
            .WithMessage(AppErrors.Authors.ProfileRequired.Message);
    }

    public override ApplicationError? Error => AppErrors.Authors.ProfileRequired;
}

public sealed class CreateComment(AppDbContext db) : Feature<CreateCommentRequest, CommentResponse>
{
    protected override async ValueTask<Result<CommentResponse>> ExecuteAsync(
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var post = await db.Posts
            .Where(x => x.Id == request.PostId)
            .Select(x => new { x.Id, x.Status })
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null)
            return AppErrors.Posts.NotFound;

        if (post.Status is not PostStatus.Published)
            return AppErrors.Posts.NotPublished;

        var author = await db.Authors.SingleAsync(x => x.Subject == request.Subject, cancellationToken);
        var comment = new Comment { Body = request.Body, PostId = post.Id, AuthorId = author.Id };

        db.Comments.Add(comment);

        await db.SaveChangesAsync(cancellationToken);

        return new CommentResponse(comment.Id, comment.PostId, comment.Body, author.Handle, comment.CreatedAt);
    }
}
