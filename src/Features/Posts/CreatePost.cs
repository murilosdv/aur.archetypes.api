using System;
using System.Threading;
using System.Threading.Tasks;
using Aurore.Archetypes.Api.Core.Domain;
using Aurore.Archetypes.Api.Core.Domain.Entities;
using Aurore.Archetypes.Api.Core.Errors;
using Aurore.Archetypes.Api.Features.Posts.Shared;
using Aurore.Archetypes.Api.Infra.Contexts;
using Aurore.Foundation.Core.Errors;
using Aurore.Foundation.Core.Features;
using Aurore.Foundation.Core.Results;
using Aurore.Foundation.Core.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Aurore.Archetypes.Api.Features.Posts;

public sealed record CreatePostRequest(string Subject, string Title, string Body);

internal sealed class CreatePostAuthorValidator : FeatureValidator<CreatePostRequest>
{
    public CreatePostAuthorValidator(AppDbContext db)
    {
        RuleFor(x => x.Subject)
            .MustAsync((subject, cancellationToken) => db.Authors.AnyAsync(x => x.Subject == subject, cancellationToken))
            .WithMessage(AppErrors.Authors.ProfileRequired.Message);
    }

    public override ApplicationError? Error => AppErrors.Authors.ProfileRequired;
}

public sealed class CreatePost(AppDbContext db) : Feature<CreatePostRequest, PostResponse>
{
    protected override async ValueTask<Result<PostResponse>> ExecuteAsync(
        CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        var author = await db.Authors.SingleAsync(x => x.Subject == request.Subject, cancellationToken);
        var id = Guid.CreateVersion7();
        var post = new Post { Id = id, Slug = PostSlug.Create(request.Title, id), Title = request.Title, Body = request.Body, AuthorId = author.Id };

        db.Posts.Add(post);

        await db.SaveChangesAsync(cancellationToken);

        return new PostResponse(post.Id, post.Slug, post.Title, post.Body, post.Status, author.Handle, post.PublishedAt, post.CreatedAt, post.UpdatedAt);
    }
}
