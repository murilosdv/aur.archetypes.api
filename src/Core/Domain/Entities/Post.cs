using System;
using System.Collections.Generic;
using Aurore.Foundation.Core.Entities;

namespace Aurore.Archetypes.Api.Core.Domain.Entities;

public sealed class Post : SoftDeletableEntity
{
    public required string Slug { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public PostStatus Status { get; set; } = PostStatus.Draft;

    public DateTimeOffset? PublishedAt { get; set; }

    public Guid AuthorId { get; init; }

    public Author Author { get; init; } = null!;

    public ICollection<Comment> Comments { get; init; } = [];
}
